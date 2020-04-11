using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RusAlTestApp.Data;
using RusAlTestApp.Data.Models;
using RusAlTestApp.ViewModels;

namespace RusAlTestApp.Services
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly ApplicationContext _applicationContext;

        public RegistrationsService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task CreateRegistration(RegistrationViewModel registrationViewModel)
        {
            var registration = MapRegistration(registrationViewModel);

            var updatedItem = (await _applicationContext.Registrations.AddAsync(registration)).Entity;
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<RegistrationPageData> GetRegistrationPageDataAsync()
        {
            var colors = await _applicationContext.Colors.Select(x => new SelectBoxModel()
            {
                Id = x.ColorId,
                Text = x.Name
            }).ToListAsync();

            var drinks = await _applicationContext.Drinks.Select(x => new SelectBoxModel()
            {
                Id = x.DrinkId,
                Text = x.Name
            }).ToListAsync();

            var model = new RegistrationPageData
            {
                Colors = colors,
                Drinks = drinks
            };

            return model;
        }


        public async Task<IEnumerable<RegistrationViewModel>> GetByFilter(FilterViewModel filter)
        {
            var query = _applicationContext.Registrations.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                .ThenBy(x => x.DateOfBirth).ThenBy(x => x.Id).Select(x => x);

            if (filter.ColorIds != null && filter.ColorIds.Any())
            {
                var idsByColor = _applicationContext.RegistrationColors.Where(x =>
                    filter.ColorIds.Contains(x.ColorId)).Select(x => x.RegistrationId);

                query = query.Where(x => idsByColor.Contains(x.Id));
            }

            if (filter.DrinkIds != null && filter.DrinkIds.Any())
            {
                var idsByDrink = _applicationContext.RegistrationDrinks.Where(x =>
                    filter.DrinkIds.Contains(x.DrinkId)).Select(x => x.RegistrationId);

                query = query.Where(x => idsByDrink.Contains(x.Id));
            }

            var result = await query.Skip(filter.Skip).Take(filter.Take).Select(x => new RegistrationViewModel
            {
                Id = x.Id,
                LastName = x.LastName,
                Name = x.FirstName,
                Phone = x.Phone,
                DrinkNames = (from d in _applicationContext.Drinks
                    join rd in _applicationContext.RegistrationDrinks on d.DrinkId equals rd.DrinkId
                    where rd.RegistrationId == x.Id
                    select d.Name).ToList(),

                ColorNames = (from c in _applicationContext.Colors
                    join rc in _applicationContext.RegistrationColors on c.ColorId equals rc.ColorId
                    where rc.RegistrationId == x.Id
                    select c.Name).ToList(),
                DateOfBirthAsDate = x.DateOfBirth

            }).ToListAsync();

            DecorateModel(result);

            return result;
        }

        private static void DecorateModel(IEnumerable<RegistrationViewModel> items)
        {
            foreach (var registration in items)
            {
                registration.DateOfBirth = registration.DateOfBirthAsDate.HasValue
                    ? registration.DateOfBirthAsDate.Value.ToString("dd.MM.yyyy")
                    : string.Empty;

                registration.ColorNamesAsString = string.Join(", ", registration.ColorNames);
                registration.DrinkNamesAsString = string.Join(", ", registration.DrinkNames);
            }
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
