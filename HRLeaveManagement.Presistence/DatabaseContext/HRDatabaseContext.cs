using ClassLibrary1HRLeaveManagement.Domain;
using ClassLibrary1HRLeaveManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Presistence.DatabaseContext
{
    public class HRDatabaseContext : DbContext
    {
        public HRDatabaseContext(DbContextOptions<HRDatabaseContext> options) : base(options)
        {

        }

        public DbSet<LeaveType> leaveTypes {get; set;}
        public DbSet<LeaveAllocation> leaveAllocations { get; set; }
        public DbSet<LeaveRequest> leaveRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this will automatically apply all configurations from the assembly where the HRDatabaseContext is defined
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRDatabaseContext).Assembly);
            //if you want to apply configurations manually, you can do so like this:
            //modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var timestamp = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                         .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.DateModified = timestamp;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = timestamp;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
