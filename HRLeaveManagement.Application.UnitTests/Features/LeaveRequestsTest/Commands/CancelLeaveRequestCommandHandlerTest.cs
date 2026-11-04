using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HRLeaveManagement.Application.Models;
using HRLeaveManagement.Application.UnitTests.Mocks;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveRequestsTest.Commands
{
    public class CancelLeaveRequestCommandHandlerTest
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IAppLogger<CancelLeaveRequestCommandHandler>> _mockLogger;

        public CancelLeaveRequestCommandHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            this._mockEmailSender = new Mock<IEmailSender>();
            this._mockLogger = new Mock<IAppLogger<CancelLeaveRequestCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_SetsCancelledTrueAndSendsEmail()
        {
            var handler = new CancelLeaveRequestCommandHandler(
                _mockLogger.Object,
                _mockEmailSender.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CancelLeaveRequestCommand { Id = 1 };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var leaveRequest = await _mockLeaveRequestRepo.Object.GetByIdAsync(1);
            leaveRequest.Cancelled.ShouldBeTrue();
            _mockEmailSender.Verify(e => e.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingId_ThrowsNotFoundException()
        {
            var handler = new CancelLeaveRequestCommandHandler(
                _mockLogger.Object,
                _mockEmailSender.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CancelLeaveRequestCommand { Id = 99 };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenEmailSenderThrowsException_LogsErrorAndReturnsUnit()
        {
            _mockEmailSender.Setup(e => e.SendEmailAsync(It.IsAny<EmailMessage>()))
                .ThrowsAsync(new Exception("Email server failed"));

            var handler = new CancelLeaveRequestCommandHandler(
                _mockLogger.Object,
                _mockEmailSender.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CancelLeaveRequestCommand { Id = 1 };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            _mockLogger.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
        }
    }
}

