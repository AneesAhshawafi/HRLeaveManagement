using HRLeaveManagement.Application.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<Employee> GetEmployee(string userId);
        Task<List<Employee>> GetEmployees();
    }
}
