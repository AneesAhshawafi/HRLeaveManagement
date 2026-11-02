using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
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
        private readonly IAppLogger<GetLeaveTypeDetailsQueryHandler> _logger;
        public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository,IAppLogger<GetLeaveTypeDetailsQueryHandler> logger)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._logger = logger;
        }

        public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
        {
            //Queryable the database
            var leaveType =  await _leaveTypeRepository.GetByIdAsync(request.id);

            //Convert the data objects to DTO objects
            var data = _mapper.Map<LeaveTypeDetailsDto>(leaveType);

            //logging the information that the leave types were retrieved successfully
            _logger.LogInformation("Leave type retrieved successfully.");

            //return List of DTO objects
            return data;
           

        }
    }
}
