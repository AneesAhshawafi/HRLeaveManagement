namespace HRLeaveManagement.BlazorUI.Services.Base
{
    public class Response<T>
    {
        public T Data { get; set; }
        public string ValidationErrors { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
