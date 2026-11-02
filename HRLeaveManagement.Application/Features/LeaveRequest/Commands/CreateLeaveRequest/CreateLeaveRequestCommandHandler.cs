using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        //private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        //private readonly IUserService _userService;

        public CreateLeaveRequestCommandHandler(
            IEmailSender emailSender,
            IMapper mapper,
            IAppLogger<CreateLeaveRequestCommandHandler> logger,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository
            //ILeaveAllocationRepository leaveAllocationRepository,
            //IUserService userService
            )
        {
            _emailSender = emailSender;
            _mapper = mapper;
            _logger = logger;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
            //this._leaveAllocationRepository = leaveAllocationRepository;
            //this._userService = userService;
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
            {

                _logger.LogError("Validation errors occurred: {Errors}", string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            // Get requesting employee's id
            //var employeeId = _userService.UserId;

            // Check on employee's allocation
            //var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

            // if allocations aren't enough, return validation error with message
            //if (allocation is null)
            //{
            //    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
            //        "You do not have any allocations for this leave type."));
                
            //    _logger.LogError("Validation errors occurred: {Errors}", string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
            //    throw new CustomValidationException("Invalid Leave Request", validationResult);
            //}

            //int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
            //if (daysRequested > allocation.NumberOfDays)
            //{
            //    validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
            //        nameof(request.EndDate), "You do not have enough days for this request"));
            //    _logger.LogError("Validation errors occurred: {Errors}", string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
            //    throw new CustomValidationException("Invalid Leave Request", validationResult);
            //}

            // Create leave request
            var leaveRequest = _mapper.Map<ClassLibrary1HRLeaveManagement.Domain.LeaveRequest>(request);
            //leaveRequest.RequestingEmployeeId = employeeId;
            leaveRequest.DateRequested = DateTime.Now;
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            // send confirmation email
            try
            {
                var email = new EmailMessage
                {
                    To = "4nis.dev@gmail.com", /* Get email from employee record */
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                        $"has been submitted successfully.",
                    Subject = "Leave Request Submitted"
                };

                await _emailSender.SendEmailAsync(email);
            }
            catch (Exception)
            {
                //// Log or handle error,
            }

            return Unit.Value;
        }
    }
}
