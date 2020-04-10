
using System.ComponentModel.DataAnnotations;

namespace RusAlTestApp.Data.Models
{
    public class Drink
    {
        [Key]
        public int DrinkId { get; set; }

        public string Name { get; set; }
    }
}
