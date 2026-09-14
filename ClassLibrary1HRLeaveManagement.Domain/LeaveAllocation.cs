using ClassLibrary1HRLeaveManagement.Domain.Common;

namespace ClassLibrary1HRLeaveManagement.Domain
{
    public class LeaveAllocation : BaseEntity
    {

        public LeaveType LeaveType { get; set; } = new LeaveType();
        public int LeaveTypeId { get; set; }
        public int NumberOfDays { get; set; }
        public int Period { get; set; }
        //public DateTime DateCreated { get; set; }
        //public string EmployeeId { get; set; } = String.Empty;
    }
}