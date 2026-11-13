using AutoMapper;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;

public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand, Unit>
{
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IAppLogger<ChangeLeaveRequestApprovalCommandHandler> _logger;
    //private readonly ILeaveAllocationRepository _leaveAllocationRepository;

    public ChangeLeaveRequestApprovalCommandHandler(
         ILeaveRequestRepository leaveRequestRepository, 
         ILeaveTypeRepository leaveTypeRepository, 
         //ILeaveAllocationRepository leaveAllocationRepository,
         IAppLogger<ChangeLeaveRequestApprovalCommandHandler> logger,
         IMapper mapper, 
         IEmailSender emailSender)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        //this._leaveAllocationRepository = leaveAllocationRepository;
        _logger = logger;
        _mapper = mapper;
        this._emailSender = emailSender;
    }

    public async Task<Unit> Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

        if (leaveRequest is null)
        {
            _logger.LogError($"Leave request with ID {request.Id} not found.");
            throw new NotFoundException(nameof(LeaveRequest), request.Id);
        }
        
        leaveRequest.Approved = request.Approved;
        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        // if request is approved, get and update the employee's allocations
        //if (request.Approved)
        //{
        //    int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
        //    var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId);
        //    allocation.NumberOfDays -= daysRequested;

        //    await _leaveAllocationRepository.UpdateAsync(allocation);
        //}

        // send confirmation email
        try
        {
            var email = new EmailMessage
            {
                To = "4nis.dev@gmail.com", /* Get email from employee record */
                Body = $"The approval status for your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been updated.",
                Subject = "Leave Request Approval Status Updated"
            };
            await _emailSender.SendEmailAsync(email);
        }
        catch (Exception)
        {
            // log error
            _logger.LogError($"Error sending email for leave request {leaveRequest.Id}.");
        }

        return Unit.Value;
    }
}
