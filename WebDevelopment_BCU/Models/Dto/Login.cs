using System.ComponentModel.DataAnnotations;

namespace WebDevelopment_BCU.Models.Dto
{
    public class Login
    {
        [Required(ErrorMessage = "UserName Required")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Required")]

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
