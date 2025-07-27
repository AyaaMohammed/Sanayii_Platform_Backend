using System.ComponentModel.DataAnnotations;

namespace Sanayii.Core.DTOs.AccountDTOs
{
    public class ConfirmPhoneNumberDto
    {
        [Phone(ErrorMessage = "Please Enter a Valid Phone Number")]
        [Required(ErrorMessage = "Please Enter Phone Number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
