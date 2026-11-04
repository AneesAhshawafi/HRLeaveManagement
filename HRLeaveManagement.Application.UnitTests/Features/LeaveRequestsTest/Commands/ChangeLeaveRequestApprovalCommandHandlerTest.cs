using AutoMapper;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.MappingProfiles;
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
    public class ChangeLeaveRequestApprovalCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IAppLogger<ChangeLeaveRequestApprovalCommandHandler>> _mockLogger;

        public ChangeLeaveRequestApprovalCommandHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            this._mockEmailSender = new Mock<IEmailSender>();
            this._mockLogger = new Mock<IAppLogger<ChangeLeaveRequestApprovalCommandHandler>>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveRequestProfiles>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesApprovalStatusAndSendsEmail()
        {
            var handler = new ChangeLeaveRequestApprovalCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object,
                _mapper,
                _mockEmailSender.Object);

            var command = new ChangeLeaveRequestApprovalCommand
            {
                Id = 1,
                Approved = true
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var leaveRequest = await _mockLeaveRequestRepo.Object.GetByIdAsync(1);
            leaveRequest.Approved.ShouldBe(true);
            _mockEmailSender.Verify(e => e.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingId_ThrowsNotFoundException()
        {
            var handler = new ChangeLeaveRequestApprovalCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object,
                _mapper,
                _mockEmailSender.Object);

            var command = new ChangeLeaveRequestApprovalCommand
            {
                Id = 99,
                Approved = true
            };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenEmailSenderThrowsException_LogsErrorAndReturnsUnit()
        {
            _mockEmailSender.Setup(e => e.SendEmailAsync(It.IsAny<EmailMessage>()))
                .ThrowsAsync(new Exception("Email server failed"));

            var handler = new ChangeLeaveRequestApprovalCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object,
                _mapper,
                _mockEmailSender.Object);

            var command = new ChangeLeaveRequestApprovalCommand
            {
                Id = 1,
                Approved = true
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            _mockLogger.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Validate_NullApprovalStatus_FailsValidation()
        {
            var validator = new ChangeLeaveRequestApprovalCommandValidator();
            var command = new ChangeLeaveRequestApprovalCommand
            {
                Id = 1,
                Approved = false
            };

            var validationResult = await validator.ValidateAsync(command);
            validationResult.IsValid.ShouldBeTrue();
        }
    }
}

