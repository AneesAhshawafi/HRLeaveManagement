using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Create
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ILeaveTypeService LeaveTypeService { get; set; } = default!;

        protected LeaveTypeVM LeaveType { get; set; } = new();
        public string Message { get; private set; } = string.Empty;
        public string ValidationErrors { get; private set; } = string.Empty;
        protected bool IsSubmitting { get; private set; }

        protected async Task CreateLeaveType()
        {
            IsSubmitting = true;
            Message = string.Empty;
            ValidationErrors = string.Empty;

            var response = await LeaveTypeService.CreateLeaveType(LeaveType);
            if (response.Success)
            {
                NavigationManager.NavigateTo("/leave-types/");
                return;
            }

            Message = response.Message;
            ValidationErrors = response.ValidationErrors;
            IsSubmitting = false;
        }
    }
}
