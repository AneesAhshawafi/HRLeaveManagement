using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes
{
    //public class GetLeaveTypesQuery : IRequest<List<LeaveTypeDto>>
    //{
    //}
    public record GetLeaveTypesQuery:IRequest<List<LeaveTypeDto>>;
}
