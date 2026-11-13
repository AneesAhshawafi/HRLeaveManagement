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
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "caf0b8e2-1c3a-4d5e-9f1b-2c3e4d5f6a7b",
                    RoleId = "caf0b8e2-1c3a-4d5e-9f1b-2c3e4d5f6a7c"
                },
                new IdentityUserRole<string>
                {
                    UserId = "d1e2f3a4-5b6c-7d8e-9f0a-1b2c3d4e5f6g",
                    RoleId = "d1e2f3a4-5b6c-7d8e-9f0a-1b2c3d4e5f6u"
                }
            );
        }
    }
}
