using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Presistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Presistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        public LeaveAllocationRepository(HRDatabaseContext context) : base(context)
        {
        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
            await _context.leaveAllocations.AddRangeAsync(allocations);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
            return await _context.leaveAllocations.
                AnyAsync(q => q.EmployeeId == userId
                && q.LeaveTypeId == leaveTypeId
                && q.Period == period
                );
        }

        public async Task<IReadOnlyList<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            return await _context.leaveAllocations
                .Include(q => q.LeaveType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
        {
            return await _context.leaveAllocations
                .Where(q=> q.EmployeeId == userId)
                .Include(q=> q.LeaveType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocation = await _context.leaveAllocations
                .Include(q => q.LeaveType)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
            if(leaveAllocation == null)
            {
                throw new NotFoundException(nameof(LeaveAllocation), id);
            }
            return leaveAllocation;
        }

        public async Task<LeaveAllocation> GetUserAllocation(string userId, int leaveTypeId)
        {
            var leaveAllocation =await _context.leaveAllocations
                .FirstOrDefaultAsync(q => q.EmployeeId== userId && q.LeaveTypeId == leaveTypeId);
            if(leaveAllocation == null)
            {
                throw new NotFoundException(nameof(LeaveAllocation), userId);
            }
            return leaveAllocation;
        }
    }
}
