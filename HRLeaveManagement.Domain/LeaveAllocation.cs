using ClassLibrary1HRLeaveManagement.Domain.Common;

namespace ClassLibrary1HRLeaveManagement.Domain
{
    public class LeaveAllocation : BaseEntity
    {

        public LeaveType LeaveType { get; set; } = new LeaveType();
        public int LeaveTypeId { get; set; }
        public int NumberOfDays { get; set; }
        public int Period { get; set; }

    }
}