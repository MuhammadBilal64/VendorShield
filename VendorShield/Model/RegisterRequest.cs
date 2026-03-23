using System.ComponentModel.DataAnnotations;

namespace VendorShield.Model
{
    public class RegisterRequest
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MinLength(8)]
        // Must match Identity password policy in Program.cs:
        // - at least 1 lowercase
        // - at least 1 uppercase
        // - at least 1 digit
        // - length >= 8
        [System.ComponentModel.DataAnnotations.RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include an uppercase letter, a lowercase letter, and a number.")]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
