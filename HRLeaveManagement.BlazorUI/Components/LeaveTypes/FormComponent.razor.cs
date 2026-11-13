using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Components.LeaveTypes
{
    public partial class FormComponent
    {
        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public LeaveTypeVM LeaveType { get; set; } = new();

        [Parameter]
        public string ButtonText { get; set; } = "Save";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public bool IsSubmitting { get; set; } = false;
    }
}
