using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveLeaveAllocationDetails
{
    public class LeaveAllocationDetailsDto 
    {
        public int Id { get; set; }
        public int NumberOfDays { get; set; }
        public LeaveTypeDto leaveType { get; set; }
        public int leaveTypeId { get; set; }
        public int period { get; set; }
    }
}
