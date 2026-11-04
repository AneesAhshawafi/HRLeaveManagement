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
    public class MockLeaveRequestRepository
    {
        public static Mock<ILeaveRequestRepository> GetMockLeaveRequestRepository()
        {
            var leaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest
                {
                    Id = 1,
                    LeaveTypeId = 1,
                    StartDate = DateTime.Now.AddDays(1),
                    EndDate = DateTime.Now.AddDays(5),
                    DateRequested = DateTime.Now,
                    RequestComments = "Vacation request",
                    Approved = null,
                    Cancelled = false,
                    RequestingEmployeeId = "emp-1",
                    LeaveType = new LeaveType { Id = 1, DefaultDays = 10, Name = "Test Vacation" }
                },
                new LeaveRequest
                {
                    Id = 2,
                    LeaveTypeId = 2,
                    StartDate = DateTime.Now.AddDays(10),
                    EndDate = DateTime.Now.AddDays(12),
                    DateRequested = DateTime.Now,
                    RequestComments = "Sick leave request",
                    Approved = true,
                    Cancelled = false,
                    RequestingEmployeeId = "emp-2",
                    LeaveType = new LeaveType { Id = 2, DefaultDays = 15, Name = "Test Sick" }
                }
            };

            var mockRepo = new Mock<ILeaveRequestRepository>();

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveRequests);
            mockRepo.Setup(r => r.GetLeaveRequestsWithDetails()).ReturnsAsync(leaveRequests);
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveRequests.FirstOrDefault(x => x.Id == id)!);
            mockRepo.Setup(r => r.GetLeaveRequestWithDetails(It.IsAny<int>()))
                .ReturnsAsync((int id) => leaveRequests.FirstOrDefault(x => x.Id == id)!);
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveRequest>()))
                .Returns((LeaveRequest leaveRequest) =>
                {
                    if (leaveRequest.Id == 0)
                    {
                        leaveRequest.Id = leaveRequests.Any() ? leaveRequests.Max(x => x.Id) + 1 : 1;
                    }
                    leaveRequests.Add(leaveRequest);
                    return Task.CompletedTask;
                });
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveRequest>()))
                .Returns((LeaveRequest leaveRequest) =>
                {
                    var existing = leaveRequests.FirstOrDefault(x => x.Id == leaveRequest.Id);
                    if (existing != null)
                    {
                        existing.StartDate = leaveRequest.StartDate;
                        existing.EndDate = leaveRequest.EndDate;
                        existing.LeaveTypeId = leaveRequest.LeaveTypeId;
                        existing.RequestComments = leaveRequest.RequestComments;
                        existing.Approved = leaveRequest.Approved;
                        existing.Cancelled = leaveRequest.Cancelled;
                    }
                    return Task.CompletedTask;
                });
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveRequest>()))
                .Returns((LeaveRequest leaveRequest) =>
                {
                    leaveRequests.Remove(leaveRequest);
                    return Task.CompletedTask;
                });

            return mockRepo;
        }
    }
}

