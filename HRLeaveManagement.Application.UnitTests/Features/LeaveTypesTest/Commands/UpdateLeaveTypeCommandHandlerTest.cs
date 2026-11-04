using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
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

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveTypesTest.Commands
{
    public class UpdateLeaveTypeCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IAppLogger<UpdateLeaveTypeCommandHandler>> _mockLogger;

        public UpdateLeaveTypeCommandHandlerTest()
        {
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<UpdateLeaveTypeCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesLeaveTypeAndReturnsUnit()
        {
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new UpdateLeaveTypeCommand
            {
                Id = 1,
                Name = "Updated Vacation",
                DefaultDays = 20
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            var updated = await _mockLeaveTypeRepo.Object.GetByIdAsync(1);
            updated.Name.ShouldBe("Updated Vacation");
            updated.DefaultDays.ShouldBe(20);
        }

        [Fact]
        public async Task Handle_LeaveTypeNotFound_ThrowsNotFoundException()
        {
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new UpdateLeaveTypeCommand
            {
                Id = 99,
                Name = "Non Existing",
                DefaultDays = 10
            };

            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidIdOrNonExistingLeaveType_ThrowsCustomValidationException()
        {
            _mockLeaveTypeRepo.Setup(r => r.IsLeaveTypeExist(1)).ReturnsAsync(false);
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new UpdateLeaveTypeCommand
            {
                Id = 1,
                Name = "Updated Vacation",
                DefaultDays = 20
            };

            await Should.ThrowAsync<CustomValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidName_ThrowsCustomValidationException()
        {
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new UpdateLeaveTypeCommand
            {
                Id = 1,
                Name = "",
                DefaultDays = 20
            };

            await Should.ThrowAsync<CustomValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DefaultDaysOutOfRange_ThrowsCustomValidationException()
        {
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new UpdateLeaveTypeCommand
            {
                Id = 1,
                Name = "Updated Vacation",
                DefaultDays = 100
            };

            await Should.ThrowAsync<CustomValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DuplicateLeaveTypeName_ThrowsCustomValidationException()
        {
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var command = new UpdateLeaveTypeCommand
            {
                Id = 1,
                Name = "Test Sick",
                DefaultDays = 20
            };

            await Should.ThrowAsync<CustomValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}

