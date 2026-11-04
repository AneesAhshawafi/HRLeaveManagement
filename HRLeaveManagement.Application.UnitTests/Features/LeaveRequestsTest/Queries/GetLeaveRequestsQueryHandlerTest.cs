using AutoMapper;
using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetAllLeaveRequests;
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
    public class GetLeaveRequestsQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveRequestRepository> _mockLeaveRequestRepo;
        private readonly Mock<IAppLogger<GetLeaveRequestsQueryHandler>> _mockLogger;

        public GetLeaveRequestsQueryHandlerTest()
        {
            this._mockLeaveRequestRepo = MockLeaveRequestRepository.GetMockLeaveRequestRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveRequestProfiles>();
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<GetLeaveRequestsQueryHandler>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsLeaveRequestDtoList()
        {
            var handler = new GetLeaveRequestsQueryHandler(_mapper, _mockLogger.Object, _mockLeaveRequestRepo.Object);
            var result = await handler.Handle(new GetLeaveRequestsQuery(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveRequestDto>>();
            result.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Handle_WhenNoRequestsExist_ReturnsEmptyList()
        {
            _mockLeaveRequestRepo.Setup(r => r.GetLeaveRequestsWithDetails()).ReturnsAsync(new List<LeaveRequest>());
            var handler = new GetLeaveRequestsQueryHandler(_mapper, _mockLogger.Object, _mockLeaveRequestRepo.Object);
            var result = await handler.Handle(new GetLeaveRequestsQuery(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveRequestDto>>();
            result.ShouldBeEmpty();
        }
    }
}

