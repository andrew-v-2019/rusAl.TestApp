
namespace RusAlTestApp.Data.Models
{
    public class RegistrationColor
    {
        public int RegistrationId { get; set; }

        public int ColorId { get; set; }

        public Registration Registration { get; set; }

        public Color Color { get; set; }
    }
}
