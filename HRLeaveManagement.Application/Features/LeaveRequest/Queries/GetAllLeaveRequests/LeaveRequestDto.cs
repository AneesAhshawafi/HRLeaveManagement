using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class LeaveRequestDto
    {
        public LeaveTypeDto LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRequested { get; set; } 
        public bool? Approved { get; set; }
        public string RequestingEmployeeId { get; set; } = String.Empty;

    }
}
