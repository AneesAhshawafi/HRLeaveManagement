using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Features.LeaveRequestsTest.Queries
{
    public class GetLeaveRequestDetailsQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<IAppLogger<GetLeaveRequestDetailsQueryHandler>> _mockLogger;

        public GetLeaveRequestDetailsQueryHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveRequestProfiles>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<GetLeaveRequestDetailsQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ExistingId_ReturnsLeaveRequestDetailsDto()
        {
            var handler = new GetLeaveRequestDetailsQueryHandler(_mapper, _mockLogger.Object, _mockLeaveRequestRepo.Object);
            var result = await handler.Handle(new GetLeaveRequestDetailsQuery { Id = 1 }, CancellationToken.None);
            result.ShouldBeOfType<LeaveRequestDetailsDto>();
            result.LeaveTypeId.ShouldBe(1);
            result.RequestComments.ShouldBe("Vacation request");
        }

        [Fact]
        public async Task Handle_NonExistingId_ReturnsNull()
        {
            var handler = new GetLeaveRequestDetailsQueryHandler(_mapper, _mockLogger.Object, _mockLeaveRequestRepo.Object);
            var result = await handler.Handle(new GetLeaveRequestDetailsQuery { Id = 99 }, CancellationToken.None);
            result.ShouldBeNull();
        }
    }
}

