using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails
{
    public class LeaveRequestDetailsDto
    {
        public LeaveTypeDto LeaveType { get; set; }

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
