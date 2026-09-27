using AutoMapper;
using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.MappingProfiles
{
    public class LeaveTypeProfile : Profile
    {
        public LeaveTypeProfile()
        {
            CreateMap<LeaveTypeDto, LeaveType>().ReverseMap(); // tow ridercations from LeaveTypeDto to LeaveType and vice versa
            CreateMap<LeaveType, LeaveTypeDetailsDto>(); // one direction from LeaveType to LeaveTypeDetailsDto
        }

        protected internal LeaveTypeProfile(string profileName) : base(profileName)
        {
        }
    }
}
