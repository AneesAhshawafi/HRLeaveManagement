using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger;
        public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository,IAppLogger<CreateLeaveTypeCommandHandler> logger)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._logger = logger;
        }


        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // ValidatedIssuer incoming data is: {request.Name}, {request.DefaultDays}");
            var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.Errors.Any())
            {
                _logger.LogError("Validation errors in CreateLeaveTypeCommandHandler: {Errors}", validationResult.Errors);
                throw new CustomValidationException("Invalid leave type data provided.",validationResult);
            }

            //Convert to domain entity object
            var LeaveTypeToCreate = _mapper.Map<ClassLibrary1HRLeaveManagement.Domain.LeaveType>(request);
            //add to database
            await _leaveTypeRepository.CreateAsync(LeaveTypeToCreate);
            //return recored id
            return LeaveTypeToCreate.Id;
        }
    }
}
