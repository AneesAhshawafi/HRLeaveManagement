using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.Models.LeaveTypes
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        // Validation attributes
        [Required]
        [MaxLength(70, ErrorMessage = "Name must not exceed 70 characters.")]
        public String Name { get; set; } = String.Empty;
        [Required]
        [Range(2, 99, ErrorMessage = "Default Number of Days must be between 2 and 99.")]
        [Display(Name = "Default Number of Days")]
        public int DefaultDays { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
