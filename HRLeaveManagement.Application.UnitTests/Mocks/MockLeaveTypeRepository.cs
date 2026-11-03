using ClassLibrary1HRLeaveManagement.Domain;
using HRLeaveManagement.Application.Contracts.Presistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.UnitTests.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetLeaveTypeMockLeaveTypeRepository()
        {
            var leaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    Id = 1,
                    DefaultDays = 10,
                    Name = "Test Vacation"
                },
                new LeaveType
                {
                    Id = 2,
                    DefaultDays = 15,
                    Name = "Test Sick"
                },
                new LeaveType
                {
                    Id = 3,
                    DefaultDays = 15,
                    Name = "Test Maternity"
                }

            };
            var mockLeaveTypesRepo = new Mock<ILeaveTypeRepository>();
            mockLeaveTypesRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveTypes);
            mockLeaveTypesRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    leaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });

            return mockLeaveTypesRepo;
        }
    }
}


