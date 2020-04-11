using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RusAlTestApp.Data;
using RusAlTestApp.Data.Models;
using RusAlTestApp.Web.Models;


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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IEnumerable<RegistrationViewModel>> Get(FilterViewModel filter)
        {
            var query = _context.Registrations.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                .ThenBy(x => x.DateOfBirth).ThenBy(x => x.Id).Select(x => x);

            if (filter.ColorIds != null && filter.ColorIds.Any())
            {
                var idsByColor = _context.RegistrationColors.Where(x =>
                    filter.ColorIds.Contains(x.ColorId)).Select(x => x.RegistrationId);

                query = query.Where(x => idsByColor.Contains(x.Id));
            }

            if (filter.DrinkIds != null && filter.DrinkIds.Any())
            {
                var idsByDrink = _context.RegistrationDrinks.Where(x =>
                    filter.DrinkIds.Contains(x.DrinkId)).Select(x => x.RegistrationId);

                query = query.Where(x => idsByDrink.Contains(x.Id));
            }

            var result = await query.Skip(filter.Skip).Take(filter.Take).Select(x => new RegistrationViewModel
            {
                Id = x.Id,
                LastName = x.LastName,
                Name = x.FirstName,
                Phone = x.Phone,
                DrinkNames = (from d in _context.Drinks
                              join rd in _context.RegistrationDrinks on d.DrinkId equals rd.DrinkId
                              where rd.RegistrationId == x.Id
                              select d.Name).ToList(),

                ColorNames = (from c in _context.Colors
                              join rc in _context.RegistrationColors on c.ColorId equals rc.ColorId
                              where rc.RegistrationId == x.Id
                              select c.Name).ToList(),
                DateOfBirthAsDate = x.DateOfBirth

            }).ToListAsync();

            foreach (var registration in result)
            {
                registration.DateOfBirth = registration.DateOfBirthAsDate.HasValue
                    ? registration.DateOfBirthAsDate.Value.ToString("dd.MM.yyyy")
                    : string.Empty;

                registration.ColorNamesAsString = string.Join(", ", registration.ColorNames);
                registration.DrinkNamesAsString = string.Join(", ", registration.DrinkNames);
            }

            return result;
        }



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
                : (DateTime?)null;

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
