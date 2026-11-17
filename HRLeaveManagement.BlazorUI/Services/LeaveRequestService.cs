using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.JSInterop;

namespace HRLeaveManagement.BlazorUI.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        public LeaveRequestService(IClient client, ILocalStorageService localStorageService) : base(client, localStorageService)
        {
        }
    }
}
