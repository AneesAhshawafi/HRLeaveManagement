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
    public class MockLeaveAllocationRepository
    {
        public static Mock<ILeaveAllocationRepository> GetMockLeaveAllocationRepository()
        {
            var leaveAllocations = new List<LeaveAllocation>
            {
                new LeaveAllocation
                {
                    Id = 1,
                    NumberOfDays = 10,
                    LeaveTypeId = 1,
                    Period = DateTime.Now.Year,
                    LeaveType = new LeaveType { Id = 1, DefaultDays = 10, Name = "Test Vacation" }
                },
                new LeaveAllocation
                {
                    Id = 2,
                    NumberOfDays = 15,
                    LeaveTypeId = 2,
                    Period = DateTime.Now.Year,
                    LeaveType = new LeaveType { Id = 2, DefaultDays = 15, Name = "Test Sick" }
                }
            };

            var mockRepo = new Mock<ILeaveAllocationRepository>();

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveAllocations);
            mockRepo.Setup(r => r.GetLeaveAllocationsWithDetails()).ReturnsAsync(leaveAllocations);
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveAllocations.FirstOrDefault(x => x.Id == id)!);
            mockRepo.Setup(r => r.GetLeaveAllocationWithDetails(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveAllocations.FirstOrDefault(x => x.Id == id)!);
            mockRepo.Setup(r => r.IsLeaveAllocationExist(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveAllocations.Any(x => x.Id == id));
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveAllocation>()))
                .Returns((LeaveAllocation allocation) =>
                {
                    if (allocation.Id == 0)
                    {
                        allocation.Id = leaveAllocations.Any() ? leaveAllocations.Max(x => x.Id) + 1 : 1;
                    }
                    leaveAllocations.Add(allocation);
                    return Task.CompletedTask;
                });
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveAllocation>()))
                .Returns((LeaveAllocation allocation) =>
                {
                    var existing = leaveAllocations.FirstOrDefault(x => x.Id == allocation.Id);
                    if (existing != null)
                    {
                        existing.NumberOfDays = allocation.NumberOfDays;
                        existing.Period = allocation.Period;
                        existing.LeaveTypeId = allocation.LeaveTypeId;
                    }
                    return Task.CompletedTask;
                });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveAllocation>()))
                .Returns((LeaveAllocation allocation) =>
                {
                    leaveAllocations.Remove(allocation);
                    return Task.CompletedTask;
                });

            return mockRepo;
        }
    }
}

