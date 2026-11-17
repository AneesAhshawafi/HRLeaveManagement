using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.JSInterop;

namespace HRLeaveManagement.BlazorUI.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        public LeaveAllocationService(IClient client, ILocalStorageService localStorageService) : base(client, localStorageService)
        {
        }

        public async Task<Response<Guid>> CreateLeaveAllocations(int leaveTypeId)
        {
            try
            {
                var createLeaveAllocation = new CreateLeaveAllocationCommand
                {
                    LeaveTypeId = leaveTypeId
                };
                await _client.LeaveAllocationsPOSTAsync(createLeaveAllocation);
                return new Response<Guid>
                {
                    Success = true,
                    Message = "Allocation Successful"
                };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }
    }
}
