using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using System.ComponentModel.DataAnnotations;

namespace HRLeaveManagement.BlazorUI.Models.LeaveRequests
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }

        public LeaveTypeVM LeaveType { get; set; }
        public EmployeeVM Employee { get; set; }
        public int LeaveTypeId { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        // [Display(Name = "Date Actioned")]
        // public DateTime DateActionted { get; set; }
        [Display(Name = "Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

        [Display(Name = "Approval Status")]
        public bool? Approved { get; set; }

        public bool Cancelled { get; set; }

        public string RequestingEmployeeId { get; set; } = String.Empty;
    }
}
