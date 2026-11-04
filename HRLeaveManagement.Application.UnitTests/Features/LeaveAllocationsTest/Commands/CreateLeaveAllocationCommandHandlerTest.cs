using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveAllocationsTest.Commands
{
    public class CreateLeaveAllocationCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepo;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IAppLogger<CreateLeaveAllocationCommandHandler>> _mockLogger;

        public CreateLeaveAllocationCommandHandlerTest()
        {
            this._mockLeaveAllocationRepo = MockLeaveAllocationRepository.GetMockLeaveAllocationRepository();
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveAllocationProfile>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<CreateLeaveAllocationCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesAllocationAndReturnsId()
        {
            var handler = new CreateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new CreateLeaveAllocationCommand { LeaveTypeId = 1 };

            var result = await handler.Handle(command, CancellationToken.None);

            var allocations = await _mockLeaveAllocationRepo.Object.GetAllAsync();
            allocations.Count.ShouldBe(3);
            result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_InvalidLeaveTypeId_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new CreateLeaveAllocationCommand { LeaveTypeId = 0 };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var allocations = await _mockLeaveAllocationRepo.Object.GetAllAsync();
            allocations.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_LeaveTypeDoesNotExist_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new CreateLeaveAllocationCommand { LeaveTypeId = 99 };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var allocations = await _mockLeaveAllocationRepo.Object.GetAllAsync();
            allocations.Count.ShouldBe(2);
        }
    }
}

