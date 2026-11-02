using AutoMapper;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _logger;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public UpdateLeaveRequestCommandHandler(
         ILeaveRequestRepository leaveRequestRepository,
         ILeaveTypeRepository leaveTypeRepository,
         IMapper mapper, IEmailSender emailSender,
         IAppLogger<UpdateLeaveRequestCommandHandler> logger)
    {
        this._leaveRequestRepository = leaveRequestRepository;
        this._leaveTypeRepository = leaveTypeRepository;
        this._mapper = mapper;
        this._emailSender = emailSender;
        this._logger = logger;
    }

    public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

        if (leaveRequest is null)
        {
            
            _logger.LogError($"Leave request with ID {request.Id} not found.");
            throw new NotFoundException(nameof(LeaveRequest), request.Id);
        }


        var validator = new UpdateLeaveRequestCommandValidator(_leaveTypeRepository, _leaveRequestRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            //formating errors for logging and exception throwing
            var formattedErrors = string.Join("; ", validationResult.Errors
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

            _logger.LogError("Validation errors occurred: {Errors}", formattedErrors);

            throw new BadRequestException("Invalid Leave Request", validationResult);
        }

        _mapper.Map(request, leaveRequest);

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        try
        {
            // send confirmation email
            var email = new EmailMessage
            {
                To = "4nis.dev@gmail.com", /* Get email from employee record */
                Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                        $"has been updated successfully.",
                Subject = "Leave Request Updated"
            };

            await _emailSender.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.Message);
        }

        return Unit.Value;
    }
}
