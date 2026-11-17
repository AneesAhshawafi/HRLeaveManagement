using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRLeaveManagement.BlazorUI.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this._localStorageService = localStorageService;
            this._jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claimsIdentity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(claimsIdentity);
            var token = await _localStorageService.GetItemAsync("token");
            if (token == null)
            {
                return new AuthenticationState(user);
            }
            var savedToken = token;
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);

            if (tokenContent.ValidTo < DateTime.UtcNow)
            {
                await _localStorageService.RemoveItemAsync("token");
                return new AuthenticationState(user);
            }

            var claims = await GetClaims();
            claimsIdentity = new ClaimsIdentity(claims, "jwt");
            user = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(user);
        }
        public async Task LoggedIn()
        {
            var claims = await GetClaims();

            // الترتيب: ClaimsPrincipal (المستخدم/المحفظة) -> ClaimsIdentity (بطاقة الهوية) -> Claims (البيانات والصلاحيات).
            // تمرير أي نص غير فارغ مثل "jwt" كـ AuthenticationType ضروري لتصبح IsAuthenticated == true (أما null أو "" فيجعلها false).
            var claimsIdentity = new ClaimsIdentity(claims, "jwt");

            // تغليف الهوية في ClaimsPrincipal (يمثل كائن User في Blazor وقد يحمل أكثر من هوية).
            var user = new ClaimsPrincipal(claimsIdentity);

            // تغليف المستخدم في AuthenticationState كـ Task جاهز لتمريره لدالة الإشعار.
            var authState = Task.FromResult(new AuthenticationState(user));

            // إشعار مكونات Blazor بتغيّر حالة الدخول لتحديث الواجهة تلقائيًا بدون Refresh.
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoggedOut()
        {
            await _localStorageService.RemoveItemAsync("token");
            var claimsIdentity = new ClaimsIdentity();
            var nobody = new ClaimsPrincipal(claimsIdentity);
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var savedToken = await _localStorageService.GetItemAsync("token");
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(type: ClaimTypes.Name, value: tokenContent.Subject));
            return claims;
        }
    }
}
