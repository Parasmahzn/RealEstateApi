using System.ComponentModel.DataAnnotations;

namespace RealEstateApi.DTOs
{
    public class Register
    {
        [Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{7,29}$", ErrorMessage = "Invalid Name")
        , MaxLength(150, ErrorMessage = "Name should be less than 150 characters")]
        public string Name { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required")]
        public string Phone { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
    public class Login
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
