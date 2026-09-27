using AutoMapper;
using HRLeaveManagement.Application.Contracts.Presistence;
using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }


        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // ValidatedIssuer incoming data is: {request.Name}, {request.DefaultDays}");

            //Convert to domain entity object
            var LeaveTypeToCreate = _mapper.Map<ClassLibrary1HRLeaveManagement.Domain.LeaveType>(request);
            //add to database
            await _leaveTypeRepository.CreateAsync(LeaveTypeToCreate);
            //return recored id
            return LeaveTypeToCreate.Id;
        }
    }
}
