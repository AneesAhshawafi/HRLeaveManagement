using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Details
    {
        [Inject]
        public ILeaveTypeService LeaveTypeService { get; set; } = default!;

        [Parameter]
        public int id { get; set; }

        protected LeaveTypeVM? LeaveType { get; set; }
        public string Message { get; private set; } = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            Message = string.Empty;
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
    }
}
