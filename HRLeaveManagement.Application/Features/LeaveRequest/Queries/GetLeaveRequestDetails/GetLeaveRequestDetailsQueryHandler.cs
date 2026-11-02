using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails
{
    public class GetLeaveRequestDetailsQueryHandler : IRequestHandler<GetLeaveRequestDetailsQuery, LeaveRequestDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly IAppLogger<GetLeaveRequestDetailsQueryHandler> _logger;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        public GetLeaveRequestDetailsQueryHandler(IMapper mapper,IAppLogger<GetLeaveRequestDetailsQueryHandler> logger,ILeaveRequestRepository leaveRequestRepository)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailsQuery request, CancellationToken cancellationToken)
        {
            //Query the leave request from the database
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            //Convert the data objects to DTO objects
            var data =_mapper.Map<LeaveRequestDetailsDto>(leaveRequest);

            //logging the information that the leave request was retrieved successfully
            _logger.LogInformation("LeaveRequest was retrieved successfully");

            //return the DTO object
            return data;
        }
    }
}
