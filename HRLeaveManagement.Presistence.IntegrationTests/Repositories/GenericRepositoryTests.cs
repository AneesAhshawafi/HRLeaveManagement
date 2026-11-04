using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Presistence.DatabaseContext;
using HRLeaveManagement.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HRLeaveManagement.Presistence.IntegrationTests.Repositories
{
    public class GenericRepositoryTests
    {
        private readonly HRDatabaseContext _context;
        private readonly GenericRepository<LeaveType> _repository;

        public GenericRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new HRDatabaseContext(dbOptions);
            _repository = new GenericRepository<LeaveType>(_context);
        }

        [Fact]
        public async Task CreateAsync_ValidEntity_PersistsToDatabase()
        {
            var leaveType = new LeaveType
            {
                Id = 1,
                Name = "Test Vacation",
                DefaultDays = 10
            };

            await _repository.CreateAsync(leaveType);

            var all = await _repository.GetAllAsync();
            all.Count.ShouldBe(1);
            all[0].Name.ShouldBe("Test Vacation");
            all[0].DateCreated.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllAsync_WhenEntitiesExist_ReturnsAllEntities()
        {
            await _repository.CreateAsync(new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 });
            await _repository.CreateAsync(new LeaveType { Id = 2, Name = "Sick", DefaultDays = 15 });

            var result = await _repository.GetAllAsync();

            result.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoEntitiesExist_ReturnsEmptyList()
        {
            var result = await _repository.GetAllAsync();

            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsEntity()
        {
            await _repository.CreateAsync(new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 });

            var result = await _repository.GetByIdAsync(1);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(1);
            result.Name.ShouldBe("Vacation");
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ThrowsException()
        {
            await Should.ThrowAsync<Exception>(async () =>
                await _repository.GetByIdAsync(99));
        }

        [Fact]
        public async Task UpdateAsync_ExistingEntity_UpdatesEntityInDatabase()
        {
            var leaveType = new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 };
            await _repository.CreateAsync(leaveType);
            _context.ChangeTracker.Clear();

            var updatedLeaveType = new LeaveType { Id = 1, Name = "Updated Vacation", DefaultDays = 20 };
            await _repository.UpdateAsync(updatedLeaveType);

            var result = await _repository.GetByIdAsync(1);
            result.Name.ShouldBe("Updated Vacation");
            result.DefaultDays.ShouldBe(20);
        }

        [Fact]
        public async Task DeleteAsync_ExistingEntity_RemovesEntityFromDatabase()
        {
            var leaveType = new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 };
            await _repository.CreateAsync(leaveType);

            await _repository.DeleteAsync(leaveType);

            var all = await _repository.GetAllAsync();
            all.ShouldBeEmpty();
        }
    }
}

