using ClassLibrary1HRLeaveManagement.Domain;

namespace HRLeaveManagement.Application.Contracts.Presistence
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        Task<bool> IsLeaveTypeUnique(string name);
        Task<bool> IsLeaveTypeExist(int id);
    }


}
