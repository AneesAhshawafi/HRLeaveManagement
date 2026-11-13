using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ILeaveTypeService LeaveTypeService { get; set; }
        [Inject]
        public ILeaveAllocationService LeaveAllocationService { get; set; } = default!;
        public List<LeaveTypeVM> LeaveTypes { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public bool Success { get; private set; }
        protected void CreateLeaveType()
        {
            NavigationManager.NavigateTo("/leaveTypes/create");
        }
        protected async Task AllocateLeaveType(int id)
        {
            var response = await LeaveAllocationService.CreateLeaveAllocations(id);
            Message = response.Message;
            Success = response.Success;
        }

        protected void DetailsLeaveType(object id)
        {
            NavigationManager.NavigateTo($"/leaveTypes/details/{id}");
        }

        protected void EditLeaveType(object id)
        {
            NavigationManager.NavigateTo($"/leaveTypes/edit/{id}");
        }

        protected async Task DeleteLeaveType(int id)
        {
            var response = await LeaveTypeService.DeleteLeaveType(id);
            if (response.Success)
            {
                Message = response.Message;
                Success = true;
                LeaveTypes = await LeaveTypeService.GetLeaveTypes();
            }
            else
            {
                Message = response.Message;
                Success = false;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            LeaveTypes = await LeaveTypeService.GetLeaveTypes();
        }
    }
}