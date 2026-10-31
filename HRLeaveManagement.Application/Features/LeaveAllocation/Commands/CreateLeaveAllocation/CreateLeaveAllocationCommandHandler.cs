using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<CreateLeaveAllocationCommandHandler> _logger;
        public CreateLeaveAllocationCommandHandler(IMapper mapper,ILeaveAllocationRepository leaveAllocationRepository,ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveAllocationCommandHandler> logger)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            //Validate data and getting the validation result
            var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            //check if the validation result has any errors
            if (validationResult.Errors.Any())
            {
                //formating errors for logging and exception throwing
                var formattedErrors = string.Join("; ", validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                //logging the validation errors
                _logger.LogError($"Validation errors occurred while creating leave allocation: {formattedErrors}");
                //throwing the validation exception with the formatted errors
                throw new CustomValidationException("Invalid leave allocation data.", validationResult);
            }
            //Convert the CreateLeaveALlocationCommand Object to Domain object
            var leaveALlocationToCreate = _mapper.Map<ClassLibrary1HRLeaveManagement.Domain.LeaveAllocation>(request);

            //Get the leaveType
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);

            //assign leaveTypeId and leaveType to the leaveAllocationToCreate
            leaveALlocationToCreate.LeaveTypeId = request.LeaveTypeId;
            leaveALlocationToCreate.LeaveType = leaveType;

            //Create in the database
            await _leaveAllocationRepository.CreateAsync(leaveALlocationToCreate);
            //return the ceated leaveAllocation id 
            return Unit.Value;
        }
    }
}
