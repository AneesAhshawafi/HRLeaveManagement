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
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HRDatabaseContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsWithDetails()
        {
            var leaveRequests =await _context.leaveRequests
                .Include(q => q.LeaveType).AsNoTracking().ToListAsync();
            return leaveRequests;
        }

        public async Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
        {
            var leaveRequests = await _context.leaveRequests
                .Where(q => q.RequestingEmployeeId == userId)
                .Include(q => q.LeaveType)
                .AsNoTracking()
                .ToListAsync();
            return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
           var leaveRequest = await _context.leaveRequests
                //.Where(q => q.Id == id)
                .Include(q => q.LeaveType)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
            if(leaveRequest == null)
            {
                throw new Exception($"Leave Request with id {id} not found.");
            }
            return leaveRequest;
        }
    }
    
}
