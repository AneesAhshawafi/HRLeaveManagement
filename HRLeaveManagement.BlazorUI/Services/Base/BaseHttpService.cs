using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace HRLeaveManagement.BlazorUI.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        protected readonly ILocalStorageService _localSrorageService;
        public BaseHttpService(IClient client, ILocalStorageService localStorageService)
        {
            this._client = client;
            this._localSrorageService = localStorageService;
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

        protected async Task AddBearerToken()
        {
            var token = await _localSrorageService.GetItemAsync("token");
            if(token != null)
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
