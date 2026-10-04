using ClassLibrary1HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Presistence
{
    public interface ILeaveTypeRepository : IGenericReposistory<LeaveType>
    {
        Task<bool> IsLeaveTypeUnique(string name);
    }


}
