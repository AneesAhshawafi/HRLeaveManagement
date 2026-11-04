using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveTypesTest.Commands
{
    public class CreateLeaveTypeCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IAppLogger<CreateLeaveTypeCommandHandler>> _mockLogger;

        public CreateLeaveTypeCommandHandlerTest()
        {
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<CreateLeaveTypeCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesLeaveTypeAndReturnsId()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new CreateLeaveTypeCommand
            {
                Name = "Test Paternity",
                DefaultDays = 10
            };

            var result = await handler.Handle(command, CancellationToken.None);

            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(4);
            result.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Handle_EmptyOrNullName_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new CreateLeaveTypeCommand
            {
                Name = "",
                DefaultDays = 10
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_NameExceedsMaxLength_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new CreateLeaveTypeCommand
            {
                Name = new string('A', 71),
                DefaultDays = 10
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_DefaultDaysOutOfRange_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new CreateLeaveTypeCommand
            {
                Name = "Test Bereavement",
                DefaultDays = 1
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_DuplicateLeaveTypeName_ThrowsBadRequestException()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new CreateLeaveTypeCommand
            {
                Name = "Test Vacation",
                DefaultDays = 10
            };

            await Should.ThrowAsync<BadRequestException>(async () =>
                await handler.Handle(command, CancellationToken.None));

            var leaveTypes = await _mockLeaveTypeRepo.Object.GetAllAsync();
            leaveTypes.Count.ShouldBe(3);
        }
    }
}

