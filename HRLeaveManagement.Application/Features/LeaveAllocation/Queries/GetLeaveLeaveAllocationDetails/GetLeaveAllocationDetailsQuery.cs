using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveLeaveAllocationDetails
{
    public record GetLeaveAllocationDetailsQuery(int Id) : IRequest<LeaveAllocationDetailsDto>
    {
    }
}
