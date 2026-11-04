using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HRLeaveManagement.Application.MappingProfiles;
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
    public class UpdateLeaveAllocationCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepo;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IAppLogger<UpdateLeaveAllocationCommandHandler>> _mockLogger;

        public UpdateLeaveAllocationCommandHandlerTest()
        {
            this._mockLeaveAllocationRepo = MockLeaveAllocationRepository.GetMockLeaveAllocationRepository();
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveAllocationProfile>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<UpdateLeaveAllocationCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesAllocationAndReturnsUnit()
        {
            var handler = new UpdateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveAllocationCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                NumberOfDays = 20,
                Period = DateTime.Now.Year
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var updated = await _mockLeaveAllocationRepo.Object.GetByIdAsync(1);
            updated.NumberOfDays.ShouldBe(20);
        }

        [Fact]
        public async Task Handle_AllocationNotFound_ThrowsNotFoundException()
        {
            var handler = new UpdateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveAllocationCommand
            {
                Id = 99,
                LeaveTypeId = 1,
                NumberOfDays = 20,
                Period = DateTime.Now.Year
            };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidOrNonExistingLeaveTypeId_ThrowsBadRequestException()
        {
            var handler = new UpdateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveAllocationCommand
            {
                Id = 1,
                LeaveTypeId = 99,
                NumberOfDays = 20,
                Period = DateTime.Now.Year
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NumberOfDaysZeroOrNegative_ThrowsBadRequestException()
        {
            var handler = new UpdateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveAllocationCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                NumberOfDays = 0,
                Period = DateTime.Now.Year
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_PeriodBeforeCurrentYear_ThrowsBadRequestException()
        {
            var handler = new UpdateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveAllocationCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                NumberOfDays = 10,
                Period = DateTime.Now.Year - 1
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_AllocationIdDoesNotExistInValidator_ThrowsBadRequestException()
        {
            _mockLeaveAllocationRepo.Setup(r => r.IsLeaveAllocationExist(1)).ReturnsAsync(false);

            var handler = new UpdateLeaveAllocationCommandHandler(
                _mapper,
                _mockLeaveAllocationRepo.Object,
                _mockLeaveTypeRepo.Object,
                _mockLogger.Object);

            var command = new UpdateLeaveAllocationCommand
            {
                Id = 1,
                LeaveTypeId = 1,
                NumberOfDays = 10,
                Period = DateTime.Now.Year
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}

