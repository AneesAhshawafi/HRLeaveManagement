using AutoMapper;
using ClassLibrary1HRLeaveManagement.Domain;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.MappingProfiles
{
    public class LeaveRequestProfiles : Profile
    {
        public LeaveRequestProfiles()
        {
            CreateMap<LeaveRequestDto, LeaveRequest>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestDetailsDto>();
            CreateMap<CreateLeaveRequestCommand, LeaveRequest>();
            CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();
        }

        protected internal LeaveRequestProfiles(string profileName) : base(profileName)
        {
        }
    }
}
