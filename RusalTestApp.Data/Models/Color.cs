using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RusAlTestApp.Data.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }

        public string Name { get; set; }
    }
}
