using ClassLibrary1HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Presistence
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    { 
    Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);
    Task<IReadOnlyList<LeaveAllocation>> GetLeaveAllocationsWithDetails();
    Task<IReadOnlyList<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId);

    Task<bool> AllocationExists(string userId, int leaveTypeId, int period);
    Task AddAllocations(List<LeaveAllocation> allocations);
    Task<LeaveAllocation> GetUserAllocation(string userId, int leaveTypeId);



    }


}
