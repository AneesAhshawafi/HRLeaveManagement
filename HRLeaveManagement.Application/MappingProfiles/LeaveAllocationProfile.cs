using AutoMapper;
using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllLeaveAllocations;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveLeaveAllocationDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.MappingProfiles
{
    public class LeaveAllocationProfile : Profile
    {
        protected LeaveAllocationProfile()
        {
            CreateMap<LeaveAllocationDto, LeaveAllocation>().ReverseMap(); // two directions from LeaveAllocationDto to LeaveAllocation and vice versa
            CreateMap<LeaveAllocation, LeaveAllocationDetailsDto>(); // one direction from LeaveAllocation to LeaveAllocationDetailsDto
            CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
            //CreateMap<UpdateLeaveAllocationCommand, LeaveAllocation>();
        }

        protected internal LeaveAllocationProfile(string profileName) : base(profileName)
        {
        }
    }
}
