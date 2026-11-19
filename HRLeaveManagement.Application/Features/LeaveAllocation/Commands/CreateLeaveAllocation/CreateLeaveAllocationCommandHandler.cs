using AutoMapper;
using HRLeaveManagement.Application.Contracts.Identity;
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
        private readonly IUserService _userService;
        public CreateLeaveAllocationCommandHandler(IMapper mapper,ILeaveAllocationRepository leaveAllocationRepository,ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveAllocationCommandHandler> logger, IUserService userService)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
            _userService = userService;
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
                throw new BadRequestException("Invalid leave allocation data.", validationResult);
            }
            
            //Get the leaveType for allocations
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);

            //Get Employees 
            var employees = await _userService.GetEmployees();

            // Get the current year(Period)
            var period = DateTime.Now.Year;
            var allocations = new List<ClassLibrary1HRLeaveManagement.Domain.LeaveAllocation>();
            foreach ( var emp in employees)
            {
                var isAllocationExists = await _leaveAllocationRepository.AllocationExists(emp.Id, leaveType.Id, period);
                if(!isAllocationExists)
                {
                    allocations.Add(new ClassLibrary1HRLeaveManagement.Domain.LeaveAllocation
                    {
                        EmployeeId = emp.Id,
                        LeaveTypeId = leaveType.Id,
                        Period = period,
                        NumberOfDays = leaveType.DefaultDays
                    });
                }
            }
            if (allocations.Any())
            {

                await _leaveAllocationRepository.AddAllocations(allocations);

                //logging the successful creation of the leave allocation
                _logger.LogInformation($"Leave allocations have been assigned successfully to all employees with this leave type id: {request.LeaveTypeId}.");
            }

            //return the ceated leaveAllocation id 
            return Unit.Value;
        }
    }
}
