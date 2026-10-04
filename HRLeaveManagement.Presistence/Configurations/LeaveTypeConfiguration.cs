using ClassLibrary1HRLeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Presistence.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            //modelBuilder.Entity<LeaveType>().HasData(
            builder.HasData(
                new LeaveType
                {
                    Id = 1,
                    Name = "Vacation",
                    DefaultDays = 10,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                }
                );
            //this is the database configuration for the LeaveType entity, we are setting the Name property to be required and have a maximum length of 100 characters
            builder.Property(q => q.Name).IsRequired().HasMaxLength(100);   
        }
    }
}
