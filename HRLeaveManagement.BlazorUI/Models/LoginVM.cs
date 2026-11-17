using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.Models
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
