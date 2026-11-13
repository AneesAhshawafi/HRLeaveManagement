using HRLeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(
                new ApplicationUser
                {
                    Id = "caf0b8e2-1c3a-4d5e-9f1b-2c3e4d5f6a7b",
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "admin@localhost.com",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin@123")
                },
                new ApplicationUser
                {
                    Id = "d1e2f3a4-5b6c-7d8e-9f0a-1b2c3d4e5f6g",
                    FirstName = "System",
                    LastName = "Employee",
                    UserName = "employee@localhost.com",
                    NormalizedUserName = "EMPLOYEE",
                    Email = "employee@localhost.com",
                    NormalizedEmail = "EMPLOYEE@LOCALHOST.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Employee@123")
                }
            );
        }
    }
}
