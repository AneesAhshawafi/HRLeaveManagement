using AutoMapper;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using HRLeaveManagement.BlazorUI.Services.Base;

namespace HRLeaveManagement.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveTypeDetailsDto, LeaveTypeVM>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.HasValue ? src.DateCreated.Value.DateTime : (DateTime?)null))
                .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateModified.HasValue ? src.DateModified.Value.DateTime : (DateTime?)null));
            CreateMap<CreateLeaveTypeCommand, LeaveTypeVM>().ReverseMap();
            CreateMap<UpdateLeaveTypeCommand, LeaveTypeVM>().ReverseMap();
        }
    }
}
