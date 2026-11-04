using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;
using HRLeaveManagement.Application.UnitTests.Mocks;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveTypesTest.Commands
{
    public class DeleteLeaveTypeCommandHandlerTest
    {
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;

        public DeleteLeaveTypeCommandHandlerTest()
        {
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
        }

        [Fact]
        public async Task Handle_ExistingId_DeletesLeaveTypeAndReturnsUnit()
        {
            var handler = new DeleteLeaveTypeCommandHandler(_mockLeaveTypeRepo.Object);
            var command = new DeleteLeaveTypeCommand { Id = 1 };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_NonExistingId_ThrowsNotFoundException()
        {
            var handler = new DeleteLeaveTypeCommandHandler(_mockLeaveTypeRepo.Object);
            var command = new DeleteLeaveTypeCommand { Id = 99 };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(3);
        }
    }
}

