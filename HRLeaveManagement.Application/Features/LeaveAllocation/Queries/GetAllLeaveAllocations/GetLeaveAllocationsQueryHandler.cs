using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllLeaveAllocations
{
    public class GetLeaveAllocationsQueryHandler : IRequestHandler<GetLeaveAllocationsQuery, List<LeaveAllocationDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<GetLeaveAllocationsQueryHandler> _logger;

        public GetLeaveAllocationsQueryHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository, IAppLogger<GetLeaveAllocationsQueryHandler> logger)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _logger = logger;
        }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationsQuery request, CancellationToken cancellationToken)
        {
            //Query the date from the database 
            var leaveTypeAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails();

            //Convert the data objects to DTO objects
            var data = _mapper.Map<List<LeaveAllocationDto>>(leaveTypeAllocations);

            //logging the information that the leave allocations were retrieved successfully
            _logger.LogInformation("LeaveAllocations were retrieved successfully");

            //return List of DTO objects
            return data;
        }
    }
}
