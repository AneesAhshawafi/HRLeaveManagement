namespace HRLeaveManagement.BlazorUI.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        public BaseHttpService(IClient client)
        {
            this._client = client;

        }
        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException apiException)
        {
            switch (apiException.StatusCode)
            {
                case 400:
                    return new Response<Guid>
                    {
                        Message = "Invalid data was submitted.",
                        ValidationErrors = apiException.Response,
                        Success = false
                    };
                case 404:
                    return new Response<Guid>
                    {
                        Message = "The requested item could not be found.",
                        Success = false
                    };
                default:
                    return new Response<Guid>
                    {
                        Message = "Something went wrong, please try again.",
                        Success = false
                    };
            }
        }
    }
}
