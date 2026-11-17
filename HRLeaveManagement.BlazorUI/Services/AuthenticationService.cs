using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Providers;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace HRLeaveManagement.BlazorUI.Services
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider) : base(client, localStorageService)
        {
            this._authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                AuthRequest authRequest = new AuthRequest()
                {
                    Email = email,
                    Password = password
                };
                var authResponse = await _client.LoginAsync(authRequest);
                if (authResponse.Token != string.Empty)
                {
                    await _localSrorageService.SetItemAsync("token", authResponse.Token);
                    // set claims in Blazor and ligin state
                    // await ((ApiAuthenticationStateProvider)
                    //     _authenticationStateProvider).LoggedIn();
                    var customAuthStateProvider = (ApiAuthenticationStateProvider)_authenticationStateProvider;
                    await customAuthStateProvider.LoggedIn();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task Logout()
        {
            // remove claims in blazor and invalidate login state
            await ((ApiAuthenticationStateProvider)
                        _authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            try
            {
                RegistrationRequest registrationRequest = new RegistrationRequest()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                    Email = email,
                    Password = password
                };
                var registrationResponse = await _client.RegisterAsync(registrationRequest);
                if (!string.IsNullOrEmpty(registrationResponse.UserId))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
