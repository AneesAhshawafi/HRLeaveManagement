using ClassLibrary1HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Presistence
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    { 
        Task<LeaveRequest> GetLeaveRequestWithDetails(int id);
        Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsWithDetails();
        Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsWithDetails(string userId);

    }


}
