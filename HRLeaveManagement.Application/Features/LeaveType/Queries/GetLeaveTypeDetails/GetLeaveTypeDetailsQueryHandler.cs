using AutoMapper;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
        {
            //Queryable the database
            var leaveType =  await _leaveTypeRepository.GetByIdAsync(request.id);

            //Convert the data objects to DTO objects
            var data = _mapper.Map<LeaveTypeDetailsDto>(leaveType);

            //return List of DTO objects
            return data;
           

        }
    }
}
