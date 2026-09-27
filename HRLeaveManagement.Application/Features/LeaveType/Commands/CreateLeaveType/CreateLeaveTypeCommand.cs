using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommand : IRequest<int>
    {
        public String Name { get; set; } = String.Empty;
        public int DefaultDays { get; set; }
    }
}
