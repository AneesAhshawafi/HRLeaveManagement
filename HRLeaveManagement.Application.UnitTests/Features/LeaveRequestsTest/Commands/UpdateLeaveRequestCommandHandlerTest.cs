using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
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
    public class UpdateLeaveRequestCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IAppLogger<UpdateLeaveRequestCommandHandler>> _mockLogger;

        public UpdateLeaveRequestCommandHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            this._mockEmailSender = new Mock<IEmailSender>();
            this._mockLogger = new Mock<IAppLogger<UpdateLeaveRequestCommandHandler>>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveRequestProfiles>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesRequestAndSendsEmail()
        {
            var handler = new UpdateLeaveRequestCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mapper,
                _mockEmailSender.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(6),
                RequestComments = "Updated comments",
                Cancelled = false
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var updated = await _mockLeaveRequestRepo.Object.GetByIdAsync(1);
            updated.RequestComments.ShouldBe("Updated comments");
            _mockEmailSender.Verify(e => e.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
        }

        [Fact]
        public async Task Handle_LeaveRequestNotFound_ThrowsNotFoundException()
        {
            var handler = new UpdateLeaveRequestCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mapper,
                _mockEmailSender.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveRequestCommand
            {
                Id = 99,
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(6),
                RequestComments = "Updated comments"
            };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_StartDateAfterOrEqualEndDate_ThrowsBadRequestException()
        {
            var handler = new UpdateLeaveRequestCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mapper,
                _mockEmailSender.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(6),
                EndDate = DateTime.Now.AddDays(2),
                RequestComments = "Invalid Dates"
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidOrNonExistingLeaveTypeId_ThrowsBadRequestException()
        {
            var handler = new UpdateLeaveRequestCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mapper,
                _mockEmailSender.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                LeaveTypeId = 99,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(6),
                RequestComments = "Invalid LeaveType"
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenEmailSenderThrowsException_LogsWarningAndReturnsUnit()
        {
            _mockEmailSender.Setup(e => e.SendEmailAsync(It.IsAny<EmailMessage>()))
                .ThrowsAsync(new Exception("Email server failed"));

            var handler = new UpdateLeaveRequestCommandHandler(
                _mockLeaveRequestRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mapper,
                _mockEmailSender.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveRequestCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(6),
                RequestComments = "Updated comments"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            _mockLogger.Verify(l => l.LogWarning(It.IsAny<string>()), Times.Once);
        }
    }
}

