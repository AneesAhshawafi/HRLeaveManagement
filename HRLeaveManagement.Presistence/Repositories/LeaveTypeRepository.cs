using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Presistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Presistence.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(HRDatabaseContext context) : base(context)
        { 
        }

        public async Task<bool> IsLeaveTypeExist(int id)
        {
            return await _context.leaveTypes.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> IsLeaveTypeUnique(string name)
        {
            return await _context.leaveTypes.AnyAsync(q => q.Name == name) == false;
        }
    }
    
}
