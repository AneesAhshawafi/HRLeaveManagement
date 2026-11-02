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

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests
{
    public class GetLeaveRequestsQueryHandler : IRequestHandler<GetLeaveRequestsQuery, List<LeaveRequestDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAppLogger<GetLeaveRequestsQueryHandler> _logger;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        public GetLeaveRequestsQueryHandler(IMapper mapper,IAppLogger<GetLeaveRequestsQueryHandler> logger,ILeaveRequestRepository leaveRequestRepository)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<List<LeaveRequestDto>> Handle(GetLeaveRequestsQuery request, CancellationToken cancellationToken)
        {
            //Query the database
            var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();

            //Convert the data objects to DTO objects
            var data = _mapper.Map<List<LeaveRequestDto>>(leaveRequests);

            //logging the information that the leave requests were retrieved successfully
            _logger.LogInformation("LeaveRequests were retrieved successfully");

            //return List of DTO objects
            return data;

        }
    }
}
