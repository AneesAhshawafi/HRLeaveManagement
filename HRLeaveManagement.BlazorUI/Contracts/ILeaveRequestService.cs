using HRLeaveManagement.BlazorUI.Models.LeaveRequests;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.Contracts
{
    public interface ILeaveRequestService
    {
        Task<Response<Guid>> CreateLeaveRequest(LeaveRequestVM leaveRequest);
        Task<List<LeaveRequestVM>> GetLeaveRequests();

    }
}
