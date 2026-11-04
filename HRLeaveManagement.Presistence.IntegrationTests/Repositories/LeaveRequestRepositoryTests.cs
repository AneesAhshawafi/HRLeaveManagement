using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Presistence.DatabaseContext;
using HRLeaveManagement.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HRLeaveManagement.Presistence.IntegrationTests.Repositories
{
    public class LeaveRequestRepositoryTests
    {
        private readonly HRDatabaseContext _context;
        private readonly LeaveRequestRepository _repository;

        public LeaveRequestRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new HRDatabaseContext(dbOptions);
            _repository = new LeaveRequestRepository(_context);
        }

        private async Task SeedDataAsync()
        {
            var leaveType = new LeaveType
            {
                Id = 1,
                Name = "Vacation",
                DefaultDays = 10
            };
            await _context.leaveTypes.AddAsync(leaveType);

            var requests = new List<LeaveRequest>
            {
                new LeaveRequest
                {
                    Id = 1,
                    LeaveTypeId = 1,
                    LeaveType = leaveType,
                    RequestingEmployeeId = "emp-1",
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(5),
                    DateRequested = DateTime.UtcNow,
                    RequestComments = "Summer vacation"
                },
                new LeaveRequest
                {
                    Id = 2,
                    LeaveTypeId = 1,
                    LeaveType = leaveType,
                    RequestingEmployeeId = "emp-2",
                    StartDate = DateTime.UtcNow.AddDays(10),
                    EndDate = DateTime.UtcNow.AddDays(12),
                    DateRequested = DateTime.UtcNow,
                    RequestComments = "Short trip"
                }
            };

            await _context.leaveRequests.AddRangeAsync(requests);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        [Fact]
        public async Task GetLeaveRequestsWithDetails_ReturnsAllRequestsWithLeaveType()
        {
            await SeedDataAsync();

            var result = await _repository.GetLeaveRequestsWithDetails();

            result.Count.ShouldBe(2);
            result.All(r => r.LeaveType != null && r.LeaveType.Name == "Vacation").ShouldBeTrue();
        }

        [Fact]
        public async Task GetLeaveRequestsWithDetailsByUserId_ReturnsUserRequestsWithLeaveType()
        {
            await SeedDataAsync();

            var result = await _repository.GetLeaveRequestsWithDetails("emp-1");

            result.Count.ShouldBe(1);
            result[0].RequestingEmployeeId.ShouldBe("emp-1");
            result[0].LeaveType.ShouldNotBeNull();
            result[0].LeaveType!.Name.ShouldBe("Vacation");
        }

        [Fact]
        public async Task GetLeaveRequestWithDetails_ExistingId_ReturnsRequestWithLeaveType()
        {
            await SeedDataAsync();

            var result = await _repository.GetLeaveRequestWithDetails(1);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.RequestComments.ShouldBe("Summer vacation");
            result.LeaveType.ShouldNotBeNull();
            result.LeaveType!.Name.ShouldBe("Vacation");
        }

        [Fact]
        public async Task GetLeaveRequestWithDetails_NonExistingId_ThrowsException()
        {
            await SeedDataAsync();

            await Should.ThrowAsync<Exception>(async () =>
                await _repository.GetLeaveRequestWithDetails(99));
        }
    }
}

