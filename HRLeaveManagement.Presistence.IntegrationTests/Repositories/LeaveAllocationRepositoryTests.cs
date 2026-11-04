using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Presistence.DatabaseContext;
using HRLeaveManagement.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HRLeaveManagement.Presistence.IntegrationTests.Repositories
{
    public class LeaveAllocationRepositoryTests
    {
        private readonly HRDatabaseContext _context;
        private readonly LeaveAllocationRepository _repository;

        public LeaveAllocationRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new HRDatabaseContext(dbOptions);
            _repository = new LeaveAllocationRepository(_context);
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

            var allocations = new List<LeaveAllocation>
            {
                new LeaveAllocation
                {
                    Id = 1,
                    EmployeeId = "user-1",
                    LeaveTypeId = 1,
                    LeaveType = leaveType,
                    NumberOfDays = 10,
                    Period = 2026
                },
                new LeaveAllocation
                {
                    Id = 2,
                    EmployeeId = "user-2",
                    LeaveTypeId = 1,
                    LeaveType = leaveType,
                    NumberOfDays = 15,
                    Period = 2026
                }
            };

            await _context.leaveAllocations.AddRangeAsync(allocations);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        [Fact]
        public async Task GetLeaveAllocationsWithDetails_ReturnsAllocationsWithLeaveType()
        {
            await SeedDataAsync();

            var result = await _repository.GetLeaveAllocationsWithDetails();

            result.Count.ShouldBe(2);
            result.All(a => a.LeaveType != null && a.LeaveType.Name == "Vacation").ShouldBeTrue();
        }

        [Fact]
        public async Task GetLeaveAllocationsWithDetailsByUserId_ReturnsFilteredAllocationsWithLeaveType()
        {
            await SeedDataAsync();

            var result = await _repository.GetLeaveAllocationsWithDetails("user-1");

            result.Count.ShouldBe(1);
            result[0].EmployeeId.ShouldBe("user-1");
            result[0].LeaveType.ShouldNotBeNull();
            result[0].LeaveType.Name.ShouldBe("Vacation");
        }

        [Fact]
        public async Task GetLeaveAllocationWithDetails_ExistingId_ReturnsAllocationWithLeaveType()
        {
            await SeedDataAsync();

            var result = await _repository.GetLeaveAllocationWithDetails(1);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.LeaveType.ShouldNotBeNull();
            result.LeaveType.Name.ShouldBe("Vacation");
        }

        [Fact]
        public async Task GetLeaveAllocationWithDetails_NonExistingId_ThrowsNotFoundException()
        {
            await SeedDataAsync();

            await Should.ThrowAsync<NotFoundException>(async () =>
                await _repository.GetLeaveAllocationWithDetails(99));
        }

        [Fact]
        public async Task AllocationExists_WhenMatchingAllocationExists_ReturnsTrue()
        {
            await SeedDataAsync();

            var exists = await _repository.AllocationExists("user-1", 1, 2026);

            exists.ShouldBeTrue();
        }

        [Fact]
        public async Task AllocationExists_WhenNoMatchingAllocation_ReturnsFalse()
        {
            await SeedDataAsync();

            var exists = await _repository.AllocationExists("user-1", 1, 2099);

            exists.ShouldBeFalse();
        }

        [Fact]
        public async Task GetUserAllocation_ExistingUserAndLeaveType_ReturnsAllocation()
        {
            await SeedDataAsync();

            var result = await _repository.GetUserAllocation("user-1", 1);

            result.ShouldNotBeNull();
            result.EmployeeId.ShouldBe("user-1");
            result.NumberOfDays.ShouldBe(10);
        }

        [Fact]
        public async Task GetUserAllocation_NonExistingUserOrLeaveType_ThrowsNotFoundException()
        {
            await SeedDataAsync();

            await Should.ThrowAsync<NotFoundException>(async () =>
                await _repository.GetUserAllocation("non-existing-user", 1));
        }

        [Fact]
        public async Task IsLeaveAllocationExist_ExistingId_ReturnsTrue()
        {
            await SeedDataAsync();

            var exists = await _repository.IsLeaveAllocationExist(1);

            exists.ShouldBeTrue();
        }

        [Fact]
        public async Task IsLeaveAllocationExist_NonExistingId_ReturnsFalse()
        {
            await SeedDataAsync();

            var exists = await _repository.IsLeaveAllocationExist(99);

            exists.ShouldBeFalse();
        }

        [Fact]
        public async Task AddAllocations_ValidList_PersistsMultipleAllocations()
        {
            var leaveType = new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 };
            await _context.leaveTypes.AddAsync(leaveType);
            await _context.SaveChangesAsync();

            var newAllocations = new List<LeaveAllocation>
            {
                new LeaveAllocation
                {
                    Id = 10,
                    EmployeeId = "user-10",
                    LeaveTypeId = 1,
                    LeaveType = leaveType,
                    NumberOfDays = 10,
                    Period = 2026
                },
                new LeaveAllocation
                {
                    Id = 11,
                    EmployeeId = "user-11",
                    LeaveTypeId = 1,
                    LeaveType = leaveType,
                    NumberOfDays = 10,
                    Period = 2026
                }
            };

            await _repository.AddAllocations(newAllocations);

            var all = await _repository.GetAllAsync();
            all.Count.ShouldBe(2);
        }
    }
}

