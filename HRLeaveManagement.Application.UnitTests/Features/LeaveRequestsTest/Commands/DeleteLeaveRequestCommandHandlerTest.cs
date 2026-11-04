using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
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
    public class DeleteLeaveRequestCommandHandlerTest
    {
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<IAppLogger<DeleteLeaveRequestCommandHandler>> _mockLogger;

        public DeleteLeaveRequestCommandHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            this._mockLogger = new Mock<IAppLogger<DeleteLeaveRequestCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_DeletesRequestAndReturnsUnit()
        {
            var handler = new DeleteLeaveRequestCommandHandler(_mockLeaveRequestRepo.Object, _mockLogger.Object);
            var command = new DeleteLeaveRequestCommand { Id = 1 };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var leaveRequests = await _mockLeaveRequestRepo.Object.GetAllAsync();
            leaveRequests.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_NonExistingId_ThrowsNotFoundException()
        {
            var handler = new DeleteLeaveRequestCommandHandler(_mockLeaveRequestRepo.Object, _mockLogger.Object);
            var command = new DeleteLeaveRequestCommand { Id = 99 };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveRequests = await _mockLeaveRequestRepo.Object.GetAllAsync();
            leaveRequests.Count.ShouldBe(2);
        }
    }
}

