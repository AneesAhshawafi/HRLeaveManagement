using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Presistence.DatabaseContext;
using HRLeaveManagement.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HRLeaveManagement.Presistence.IntegrationTests.Repositories
{
    public class LeaveTypeRepositoryTests
    {
        private readonly HRDatabaseContext _context;
        private readonly LeaveTypeRepository _repository;

        public LeaveTypeRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new HRDatabaseContext(dbOptions);
            _repository = new LeaveTypeRepository(_context);
        }

        [Fact]
        public async Task IsLeaveTypeExist_ExistingId_ReturnsTrue()
        {
            await _repository.CreateAsync(new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 });

            var exists = await _repository.IsLeaveTypeExist(1);

            exists.ShouldBeTrue();
        }

        [Fact]
        public async Task IsLeaveTypeExist_NonExistingId_ReturnsFalse()
        {
            var exists = await _repository.IsLeaveTypeExist(99);

            exists.ShouldBeFalse();
        }

        [Fact]
        public async Task IsLeaveTypeUnique_UniqueName_ReturnsTrue()
        {
            await _repository.CreateAsync(new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 });

            var isUnique = await _repository.IsLeaveTypeUnique("Sick Leave");

            isUnique.ShouldBeTrue();
        }

        [Fact]
        public async Task IsLeaveTypeUnique_DuplicateName_ReturnsFalse()
        {
            await _repository.CreateAsync(new LeaveType { Id = 1, Name = "Vacation", DefaultDays = 10 });

            var isUnique = await _repository.IsLeaveTypeUnique("Vacation");

            isUnique.ShouldBeFalse();
        }
    }
}

