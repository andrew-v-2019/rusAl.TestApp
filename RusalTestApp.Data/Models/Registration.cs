using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RusAlTestApp.Data.Models
{
    public class Registration
    {
        public Registration()
        {
            RegistrationDrinks = new List<RegistrationDrink>();
        }


        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<RegistrationDrink> RegistrationDrinks { get; set; }

    }
}
