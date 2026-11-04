using AutoMapper;
using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllLeaveAllocations;
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
    public class GetLeaveAllocationsQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveAllocationRepository> _mockLeaveAllocationRepo;
        private readonly Mock<IAppLogger<GetLeaveAllocationsQueryHandler>> _mockLogger;

        public GetLeaveAllocationsQueryHandlerTest()
        {
            this._mockLeaveAllocationRepo = MockLeaveAllocationRepository.GetMockLeaveAllocationRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveAllocationProfile>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<GetLeaveAllocationsQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsLeaveAllocationDtoList()
        {
            var handler = new GetLeaveAllocationsQueryHandler(_mapper, _mockLeaveAllocationRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveAllocationsQuery(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveAllocationDto>>();
            result.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_WhenNoAllocationsExist_ReturnsEmptyList()
        {
            _mockLeaveAllocationRepo.Setup(r => r.GetLeaveAllocationsWithDetails()).ReturnsAsync(new List<LeaveAllocation>());
            var handler = new GetLeaveAllocationsQueryHandler(_mapper, _mockLeaveAllocationRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveAllocationsQuery(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveAllocationDto>>();
            result.ShouldBeEmpty();
        }
    }
}

