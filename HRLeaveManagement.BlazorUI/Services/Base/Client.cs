namespace HRLeaveManagement.BlazorUI.Services.Base
{
    public partial class Client : IClient
    {
        public HttpClient HttpClient {
             get
            {
                HttpClient _httpClient;
            }
        }
        
    }
}
