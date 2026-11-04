using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;
using HRLeaveManagement.Application.UnitTests.Mocks;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveAllocationsTest.Commands
{
    public class DeleteLeaveAllocationCommandHandlerTest
    {
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepo;
        private readonly Mock<IAppLogger<DeleteLeaveAllocationCommandHandler>> _mockLogger;

        public DeleteLeaveAllocationCommandHandlerTest()
        {
            this._mockLeaveAllocationRepo = MockLeaveAllocationRepository.GetMockLeaveAllocationRepository();
            this._mockLogger = new Mock<IAppLogger<DeleteLeaveAllocationCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_DeletesAllocationAndReturnsUnit()
        {
            var handler = new DeleteLeaveAllocationCommandHandler(_mockLeaveAllocationRepo.Object, _mockLogger.Object);
            var command = new DeleteLeaveAllocationCommand { Id = 1 };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var allocations = await _mockLeaveAllocationRepo.Object.GetAllAsync();
            allocations.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Handle_NonExistingId_ThrowsNotFoundException()
        {
            var handler = new DeleteLeaveAllocationCommandHandler(_mockLeaveAllocationRepo.Object, _mockLogger.Object);
            var command = new DeleteLeaveAllocationCommand { Id = 99 };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var allocations = await _mockLeaveAllocationRepo.Object.GetAllAsync();
            allocations.Count.ShouldBe(2);
        }
    }
}

