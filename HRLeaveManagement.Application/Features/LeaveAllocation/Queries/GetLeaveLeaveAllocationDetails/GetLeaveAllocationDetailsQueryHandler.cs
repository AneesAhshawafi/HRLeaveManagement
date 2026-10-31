using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveLeaveAllocationDetails
{
    public class GetLeaveAllocationDetailsQueryHandler : IRequestHandler<GetLeaveAllocationDetailsQuery, LeaveAllocationDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<GetLeaveAllocationDetailsQueryHandler> _logger;

        public GetLeaveAllocationDetailsQueryHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository, IAppLogger<GetLeaveAllocationDetailsQueryHandler> logger)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _logger = logger;
        }

        public async Task<LeaveAllocationDetailsDto> Handle(GetLeaveAllocationDetailsQuery request, CancellationToken cancellationToken)
        {
            //Get the leave allocation details from the database using the repository
            var leaveAllocationDetails = await _leaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id);

            //convert the data objects to DTO objects
            var leaveAllocationDetailsDto = _mapper.Map<LeaveAllocationDetailsDto>(leaveAllocationDetails);

            //logging the information that the leave allocation details were retrieved successfully
            _logger.LogInformation($"LeaveAllocation details for Id {request.Id} were retrieved successfully");

            //return the DTO object
            return leaveAllocationDetailsDto;
        }
    }
}
