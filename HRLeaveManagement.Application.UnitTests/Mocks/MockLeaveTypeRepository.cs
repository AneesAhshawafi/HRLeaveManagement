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
            mockLeaveTypesRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveTypes.FirstOrDefault(x => x.Id == id)!);
            mockLeaveTypesRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    if (leaveType.Id == 0)
                    {
                        leaveType.Id = leaveTypes.Any() ? leaveTypes.Max(x => x.Id) + 1 : 1;
                    }
                    leaveTypes.Add(leaveType);
                    return Task.CompletedTask;
                });
            mockLeaveTypesRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    var existing = leaveTypes.FirstOrDefault(x => x.Id == leaveType.Id);
                    if (existing != null)
                    {
                        existing.Name = leaveType.Name;
                        existing.DefaultDays = leaveType.DefaultDays;
                    }
                    return Task.CompletedTask;
                });
            mockLeaveTypesRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveType>()))
                .Returns((LeaveType leaveType) =>
                {
                    leaveTypes.Remove(leaveType);
                    return Task.CompletedTask;
                });
            mockLeaveTypesRepo.Setup(r => r.IsLeaveTypeUnique(It.IsAny<string>()))
                .ReturnsAsync((string name) => !leaveTypes.Any(x => x.Name == name));
            mockLeaveTypesRepo.Setup(r => r.IsLeaveTypeExist(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveTypes.Any(x => x.Id == id));

            return mockLeaveTypesRepo;
        }
    }
}
