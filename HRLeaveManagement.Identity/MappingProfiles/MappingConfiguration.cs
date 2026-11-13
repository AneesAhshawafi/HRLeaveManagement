using AutoMapper;
using HRLeaveManagement.Application.Models.Identity;
using HRLeaveManagement.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Identity.MappingProfiles
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Employee,ApplicationUser>().ReverseMap();
        }
    }
}
