using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly IAppLogger<CancelLeaveRequestCommandHandler> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public CancelLeaveRequestCommandHandler(IAppLogger<CancelLeaveRequestCommandHandler> logger, IEmailSender emailSender, ILeaveRequestRepository leaveRequestRepository)
        {
            _logger = logger;
            _emailSender = emailSender;
            _leaveRequestRepository = leaveRequestRepository;   
        }

        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            
            if(leaveRequest == null)
            {
                _logger.LogInformation($"Leave request with id {request.Id} not found.");
                throw new NotFoundException($"Leave request with id {request.Id} not found.");
            }
            leaveRequest.Cancelled = true;

            // Re-evaluate the employee's allocations for the leave type
            try
            {
                // send confirmation email
                var email = new EmailMessage
                {
                    To = "4nis.dev@gmail.com",
                    Body = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been cancelled.",
                    Subject = "Leave Request Cancelled"
                };
                await _emailSender.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending cancellation email for leave request {leaveRequest.Id}: {ex.Message}");

            }
           
            return Unit.Value;
        }
    }
}
