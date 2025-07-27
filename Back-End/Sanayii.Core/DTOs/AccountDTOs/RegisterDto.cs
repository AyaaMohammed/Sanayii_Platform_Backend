using System.ComponentModel.DataAnnotations;

namespace Sanayii.Core.DTOs.AccountDTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
        public string City { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Street name cannot exceed 200 characters.")]
        public string Street { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Government name cannot exceed 100 characters.")]
        public string Government { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one phone number is required.")]
        public List<string> PhoneNumbers { get; set; } = new List<string>();

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
