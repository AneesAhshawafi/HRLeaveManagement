using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveLeaveAllocationDetails;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveAllocationsTest.Queries
{
    public class GetLeaveAllocationDetailsQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepo;
        private readonly Mock<IAppLogger<GetLeaveAllocationDetailsQueryHandler>> _mockLogger;

        public GetLeaveAllocationDetailsQueryHandlerTest()
        {
            this._mockLeaveAllocationRepo = MockLeaveAllocationRepository.GetMockLeaveAllocationRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveAllocationProfile>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<GetLeaveAllocationDetailsQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsLeaveAllocationDetailsDto()
        {
            var handler = new GetLeaveAllocationDetailsQueryHandler(_mapper, _mockLeaveAllocationRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveAllocationDetailsQuery { Id = 1 }, CancellationToken.None);
            result.ShouldBeOfType<LeaveAllocationDetailsDto>();
            result.Id.ShouldBe(1);
            result.NumberOfDays.ShouldBe(10);
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            var handler = new GetLeaveAllocationDetailsQueryHandler(_mapper, _mockLeaveAllocationRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveAllocationDetailsQuery { Id = 99 }, CancellationToken.None);
            result.ShouldBeNull();
        }
    }
}

