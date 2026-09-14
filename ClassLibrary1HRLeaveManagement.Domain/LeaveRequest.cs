using ClassLibrary1HRLeaveManagement.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary1HRLeaveManagement.Domain
{
    public class LeaveRequest : BaseEntity
    {

        public LeaveType? LeaveType { get; set; };

        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRequested { get; set; }
        public string RequestComments { get; set; } = String.Empty;
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }


        public string RequestingEmployeeId { get; set; } = String.Empty;
    }
}