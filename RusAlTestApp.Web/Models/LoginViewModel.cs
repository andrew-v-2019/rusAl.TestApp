using System.ComponentModel.DataAnnotations;

namespace RusAlTestApp.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Имя пользователя не может быть пустым")]
        [Display(Name = "Имя пользователя")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Пароль не может быть пустым")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
    }
}
