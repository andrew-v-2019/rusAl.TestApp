using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;
using RusAlTestApp.Data;
using RusAlTestApp.Data.Models;
using RusAlTestApp.Web.Models;
using Color = RusAlTestApp.Data.Models.Color;


namespace RusAlTestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly ApplicationContext _context;

        public RegistrationsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("selectBoxValues")]
        public async Task<RegistrationPageData> GetSelectBoxValuesAsync()
        {
            var colors = await _context.Colors.Select(x => new SelectBoxModel()
            {
                Id = x.ColorId,
                Text = x.Name
            }).ToListAsync();

            var drinks = await _context.Drinks.Select(x => new SelectBoxModel()
            {
                Id = x.DrinkId,
                Text = x.Name
            }).ToListAsync();

            var model = new RegistrationPageData()
            {
                Colors = colors,
                Drinks = drinks
            };

            return model;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] RegistrationViewModel registrationViewModel)
        {
            var registration = MapRegistration(registrationViewModel);

            var updatedItem = (await _context.Registrations.AddAsync(registration)).Entity;
            await _context.SaveChangesAsync();
            return Ok();
        }

        private static Registration MapRegistration(RegistrationViewModel registrationViewModel)
        {
            var date = !string.IsNullOrWhiteSpace(registrationViewModel.DateOfBirth)
                ? DateTime.Parse(registrationViewModel.DateOfBirth)
                : (DateTime?) null;

            var registration = new Registration
            {
                FirstName = registrationViewModel.Name,
                LastName = registrationViewModel.LastName,
                Phone = registrationViewModel.Phone,
                DateOfBirth = date,
                RegistrationDrinks = MapDrinks(registrationViewModel),
                RegistrationColors = MapColors(registrationViewModel)
            };

            return registration;
        }

        private static List<RegistrationColor> MapColors(RegistrationViewModel registrationViewModel)
        {
            var colors = registrationViewModel.Colors != null
                ? registrationViewModel.Colors.Select(x => new RegistrationColor
                {
                    ColorId = x
                }).ToList()
                : new List<RegistrationColor>();

            return colors;
        }

        private static List<RegistrationDrink> MapDrinks(RegistrationViewModel registrationViewModel)
        {
            var drinks = registrationViewModel.Drinks != null
                ? registrationViewModel.Drinks.Select(x => new RegistrationDrink
                {
                    DrinkId = x
                }).ToList()
                : new List<RegistrationDrink>();

            return drinks;
        }


    }
}
