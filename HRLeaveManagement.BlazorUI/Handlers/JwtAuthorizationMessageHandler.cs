using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace HRLeaveManagement.BlazorUI.Handlers
{
    public class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;
        public JwtAuthorizationMessageHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorageService.GetItemAsync("token");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
