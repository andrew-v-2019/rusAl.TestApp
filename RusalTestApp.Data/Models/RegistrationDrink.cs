
namespace RusAlTestApp.Data.Models
{
    public class RegistrationDrink
    {
        public int RegistrationId { get; set; }

        public int DrinkId { get; set; }

        public Registration  Registration {get; set; }

        public Drink Drink {get; set; }
    }
}
