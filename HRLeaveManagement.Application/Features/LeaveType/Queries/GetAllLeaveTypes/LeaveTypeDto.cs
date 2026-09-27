using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes
{
    public class LeaveTypeDto
    {
        public int Id { get; set; }
        public String Name { get; set; } = String.Empty;
        public int DefaultDays { get; set; }
    }
}
