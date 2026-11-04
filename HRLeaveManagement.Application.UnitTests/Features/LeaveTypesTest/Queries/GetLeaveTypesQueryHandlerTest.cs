using AutoMapper;
using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveypes;
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
    public class GetLeaveTypesQueryHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepo;
        private readonly Mock<IAppLogger<GetLeavetypesQueryHandler>> _mockLogger;
        public GetLeaveTypesQueryHandlerTest()
        {
            this._mockLeaveTypeRepo = MockLeaveTypeRepository.GetLeaveTypeMockLeaveTypeRepository();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
            this._mockLogger = new Mock<IAppLogger<GetLeavetypesQueryHandler>>();
        }

        //[Fact]
        //public async Task GetLeaveTypesTest()
        //{
        //    var handler = new GetLeavetypesQueryHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
        //    var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
        //    result.ShouldBeOfType<List<LeaveTypeDto>>();
        //    result.Count.ShouldBe(3);
        //}

        [Fact]
        public async Task Handle_ValidRequest_ReturnsLeaveTypeDtoList()
        {
            var handler = new GetLeavetypesQueryHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Handle_WhenNoLeaveTypesExist_ReturnsEmptyList()
        {
            _mockLeaveTypeRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<LeaveType>());
            var handler = new GetLeavetypesQueryHandler(_mapper, _mockLeaveTypeRepo.Object, _mockLogger.Object);
            var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.ShouldBeEmpty();
        }
    }
}
