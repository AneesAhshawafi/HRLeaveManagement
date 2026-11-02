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

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<UpdateLeaveAllocationCommandHandler> _logger;
        public UpdateLeaveAllocationCommandHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository, IAppLogger<UpdateLeaveAllocationCommandHandler> logger)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            //get the original leave allocation from the database
            var leaveAllocationToUpdate = await _leaveAllocationRepository.GetByIdAsync(request.Id);

            if(leaveAllocationToUpdate == null)
            {
                //logging the error for leave allocation not found
                _logger.LogError($"Leave allocation with ID {request.Id} not found.");
                //throwing a not found exception if the leave allocation does not exist
                throw new NotFoundException(nameof(ClassLibrary1HRLeaveManagement.Domain.LeaveAllocation), request.Id);
            }
            //validate data
            var validator = new UpdateLeaveAllocationCommandValidator(_leaveTypeRepository, _leaveAllocationRepository);
            var validationResult = validator.Validate(request);

            //check if the validation result has any errors
            if(validationResult.Errors.Any())
            {
                //formating errors for logging and exception throwing
                var formattedErrors = string.Join("; ", validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

                //logging the validation errors
                _logger.LogError($"Validation errors occurred while updating leave allocation: {formattedErrors}");

                //throwing the validation exception with the formatted errors
                throw new BadRequestException("Invalid leave allocation data.", validationResult);
            }

            

            //Convert the UpdateLeaveAllocationCommand Object to Domain object
            //var leaveAllocationToUpdate = _mapper.Map<ClassLibrary1HRLeaveManagement.Domain.LeaveAllocation>(request);
            _mapper.Map(request, leaveAllocationToUpdate);

            //Update the leave allocation in the database
            await _leaveAllocationRepository.UpdateAsync(leaveAllocationToUpdate);

            //logging the successful update of the leave allocation
            _logger.LogInformation($"Leave allocation with ID {request.Id} has been updated successfully.");

            //return nothing
            return Unit.Value;


        }
    
    }
}
