using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes
{
    public class GetLeavetypesQueryHandler : IRequestHandler<GetLeaveTypesQuery, List<LeaveTypeDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<GetLeavetypesQueryHandler> _logger;
        public GetLeavetypesQueryHandler(IMapper mapper,ILeaveTypeRepository leaveTypeRepository,IAppLogger<GetLeavetypesQueryHandler> logger) 
        {
            this._leaveTypeRepository = leaveTypeRepository;
            this._mapper = mapper;
            this._logger = logger;

        }

        public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
        {
            //Queryable the database
            var leaveTypes =await _leaveTypeRepository.GetAllAsync();
             
            //Convert the data objects to DTO objects
            var data = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);

            //logging the information that the leave types were retrieved successfully
            _logger.LogInformation("LeaveTypes were retrieved successfully");

            //return List of DTO objects
            return data;
        }
    }
}
