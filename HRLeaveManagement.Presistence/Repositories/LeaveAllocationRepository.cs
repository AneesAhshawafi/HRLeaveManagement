using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Presistence.DatabaseContext;
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
    }
}
