using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.Models.LeaveTypes
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }
        // Validation attributes
        [Required]
        public String Name { get; set; } = String.Empty;
        [Required]
        [Display(Name = "Default Number of Days")]
        public int DefaultDays { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
