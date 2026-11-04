using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Presistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HRLeaveManagement.Presistence.IntegrationTests
{
    public class HRDatabaseContextTests
    {
        private readonly HRDatabaseContext _hrDatabaseContext;

        public HRDatabaseContextTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _hrDatabaseContext = new HRDatabaseContext(dbOptions);
        }

        [Fact]
        public async Task Save_SetDateCreatedValue()
        {
            // Arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation"
            };

            // Act
            await _hrDatabaseContext.leaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            // Assert
            leaveType.DateCreated.ShouldNotBeNull();
        }

        [Fact]
        public async Task Save_SetDateModifiedValue()
        {
            // Arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation"
            };

            // Act
            await _hrDatabaseContext.leaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            // Assert
            leaveType.DateModified.ShouldNotBeNull();
        }

        [Fact]
        public async Task Update_UpdatesDateModifiedAndPreservesDateCreated()
        {
            // Arrange
            var leaveType = new LeaveType
            {
                Id = 1,
                DefaultDays = 10,
                Name = "Test Vacation"
            };
            await _hrDatabaseContext.leaveTypes.AddAsync(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            var originalDateCreated = leaveType.DateCreated;
            var originalDateModified = leaveType.DateModified;

            await Task.Delay(20);

            // Act
            leaveType.Name = "Updated Vacation";
            _hrDatabaseContext.leaveTypes.Update(leaveType);
            await _hrDatabaseContext.SaveChangesAsync();

            // Assert
            leaveType.DateCreated.ShouldBe(originalDateCreated);
            leaveType.DateModified.ShouldNotBeNull();
            leaveType.DateModified.Value.ShouldBeGreaterThan(originalDateModified!.Value);
        }

        [Fact]
        public async Task EnsureCreated_SeedsDefaultVacationLeaveType()
        {
            // Act
            await _hrDatabaseContext.Database.EnsureCreatedAsync();
            var seededLeaveType = await _hrDatabaseContext.leaveTypes.FirstOrDefaultAsync(q => q.Id == 1);

            // Assert
            seededLeaveType.ShouldNotBeNull();
            seededLeaveType.Name.ShouldBe("Vacation");
            seededLeaveType.DefaultDays.ShouldBe(10);
        }
    }
}
