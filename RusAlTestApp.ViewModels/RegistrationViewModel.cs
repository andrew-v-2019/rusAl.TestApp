using System;
using System.Collections.Generic;

namespace RusAlTestApp.ViewModels
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string DateOfBirth { get; set; }

        public DateTime? DateOfBirthAsDate { get; set; }

        public List<int> Drinks { get; set; }

        public List<int> Colors { get; set; }

        public List<string> ColorNames { get; set; }
        public List<string> DrinkNames { get; set; }

        public string ColorNamesAsString { get; set; }
        public string DrinkNamesAsString { get; set; }
    }
}
