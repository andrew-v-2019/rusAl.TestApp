using System;
using System.Collections.Generic;


namespace RusAlTestApp.Web.Models
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string DateOfBirth { get; set; }

        public List<int> Drinks { get; set; }

        public List<int> Colors { get; set; }
    }
}
