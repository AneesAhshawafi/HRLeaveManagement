using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.Models.Email;
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
    public class CreateLeaveRequestCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IAppLogger<CreateLeaveRequestCommandHandler>> _mockLogger;

        public CreateLeaveRequestCommandHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            this._mockEmailSender = new Mock<IEmailSender>();
            this._mockLogger = new Mock<IAppLogger<CreateLeaveRequestCommandHandler>>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveRequestProfiles>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesRequestAndSendsEmail()
        {
            var handler = new CreateLeaveRequestCommandHandler(
                _mockEmailSender.Object,
                _mapper,
                _mockLogger.Object,
                _mockLeaveTypeRepo.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CreateLeaveRequestCommand
            {
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                RequestComments = "New Leave Request"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var leaveRequests = await _mockLeaveRequestRepo.Object.GetAllAsync();
            leaveRequests.Count.ShouldBe(3);
            _mockEmailSender.Verify(e => e.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
        }

        [Fact]
        public async Task Handle_StartDateAfterOrEqualEndDate_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveRequestCommandHandler(
                _mockEmailSender.Object,
                _mapper,
                _mockLogger.Object,
                _mockLeaveTypeRepo.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CreateLeaveRequestCommand
            {
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(1),
                RequestComments = "Invalid Dates"
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveRequests = await _mockLeaveRequestRepo.Object.GetAllAsync();
            leaveRequests.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_InvalidOrNonExistingLeaveTypeId_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveRequestCommandHandler(
                _mockEmailSender.Object,
                _mapper,
                _mockLogger.Object,
                _mockLeaveTypeRepo.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CreateLeaveRequestCommand
            {
                LeaveTypeId = 99,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                RequestComments = "Invalid LeaveType"
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveRequests = await _mockLeaveRequestRepo.Object.GetAllAsync();
            leaveRequests.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_WhenEmailSenderThrowsException_StillReturnsUnit()
        {
            _mockEmailSender.Setup(e => e.SendEmailAsync(It.IsAny<EmailMessage>()))
                .ThrowsAsync(new Exception("Email server failed"));

            var handler = new CreateLeaveRequestCommandHandler(
                _mockEmailSender.Object,
                _mapper,
                _mockLogger.Object,
                _mockLeaveTypeRepo.Object,
                _mockLeaveRequestRepo.Object);

            var command = new CreateLeaveRequestCommand
            {
                LeaveTypeId = 1,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(5),
                RequestComments = "New Leave Request"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var leaveRequests = await _mockLeaveRequestRepo.Object.GetAllAsync();
            leaveRequests.Count.ShouldBe(3);
        }
    }
}

