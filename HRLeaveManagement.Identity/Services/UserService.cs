using AutoMapper;
using HRLeaveManagement.Application.Contracts.Identity;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Employee> GetEmployee(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);
            if(employee == null)
            {
                throw new NotFoundException($"Employee with id {userId} not found.");
            }
            return _mapper.Map<Employee>(employee);
           
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync("Employee");
            var employees = _mapper.Map<List<Employee>>(users);
            return employees;

        }
    }
}
