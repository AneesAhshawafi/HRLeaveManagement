using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveTypesTest.Queries
{
    public class GetLeaveTypeDetailsQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IAppLogger<GetLeaveTypeDetailsQueryHandler>> _mockLogger;

        public GetLeaveTypeDetailsQueryHandlerTest()
        {
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<GetLeaveTypeDetailsQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsLeaveTypeDetailsDto()
        {
            var handler = new GetLeaveTypeDetailsQueryHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveTypeDetailsQuery(1), CancellationToken.None);
            result.ShouldBeOfType<LeaveTypeDetailsDto>();
            result.Id.ShouldBe(1);
            result.Name.ShouldBe("Test Vacation");
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            var handler = new GetLeaveTypeDetailsQueryHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveTypeDetailsQuery(99), CancellationToken.None);
            result.ShouldBeNull();
        }
    }
}

