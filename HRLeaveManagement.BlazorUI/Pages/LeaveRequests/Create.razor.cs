
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveRequests;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using HRLeaveManagement.BlazorUI.Services;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveRequests
{
    public partial class Create
    {
        [Inject]
        ILeaveTypeService leaveTypeService { get; set; }
        [Inject]
        ILeaveRequestService leaveRequestService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        List<LeaveTypeVM> leaveTypeVMs { get; set; } = new List<LeaveTypeVM>();
        public LeaveRequestVM LeaveRequest { get; set; } = new LeaveRequestVM();

        protected override async Task OnInitializedAsync()
        {
            leaveTypeVMs = await leaveTypeService.GetLeaveTypes();

        }

        private async Task HandleValidSubmit()
        {
            await leaveRequestService.CreateLeaveRequest(LeaveRequest);
            //NavigationManager.NavigateTo("/leave-requests/");
        }
    }
}