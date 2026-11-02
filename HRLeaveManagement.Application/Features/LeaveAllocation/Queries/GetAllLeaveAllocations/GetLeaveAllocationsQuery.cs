using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllLeaveAllocations
{
    public record GetLeaveAllocationsQuery : IRequest<List<LeaveAllocationDto>>
    {

    }
}
