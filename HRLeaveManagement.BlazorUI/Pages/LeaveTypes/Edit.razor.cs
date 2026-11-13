using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Edit
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public ILeaveTypeService LeaveTypeService { get; set; } = default!;

        [Parameter]
        public int id { get; set; }

        protected LeaveTypeVM? LeaveType { get; set; }
        public string Message { get; private set; } = string.Empty;
        public string ValidationErrors { get; private set; } = string.Empty;
        protected bool IsSubmitting { get; private set; }

        protected override async Task OnParametersSetAsync()
        {
            Message = string.Empty;
            ValidationErrors = string.Empty;
            LeaveType = null;

            try
            {
                LeaveType = await LeaveTypeService.GetLeaveTypeDetails(id);
            }
            catch (ApiException ex)
            {
                Message = ex.StatusCode == 404
                    ? "The requested leave type could not be found."
                    : "Unable to load leave type details.";
            }
            catch (Exception)
            {
                Message = "Unable to load leave type details.";
            }
        }

        protected async Task EditLeaveType()
        {
            if (LeaveType is null)
            {
                return;
            }

            IsSubmitting = true;
            Message = string.Empty;
            ValidationErrors = string.Empty;

            var response = await LeaveTypeService.UpdateLeaveType(id, LeaveType);
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
