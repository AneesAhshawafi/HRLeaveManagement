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
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HRDatabaseContext context) : base(context)
        {
        }
    }
    
}
