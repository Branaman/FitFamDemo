using System.ComponentModel.DataAnnotations;
namespace ExcitedEmu.Models
{
    public class User : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,32}$", ErrorMessage = "Password must be between 8 and 32 characters long containing a number, a letter and a special character.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
    public class LoginUser : User
    {
        public int idusers {get;set;}
    }
    public class RegisterUser : User
    {
        [Required]
        [MinLength(2)]
        public string first_name { get; set; }
        [Required]
        [MinLength(3)]
        public string last_name { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(16, ErrorMessage = "Confirm Password must be between 5 and 15 Characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Passwords do not Match")]
        public string ConfirmPassword { get; set; }
    }
}