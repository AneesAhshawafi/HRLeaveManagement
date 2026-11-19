using AutoMapper;
using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveRequests;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using HRLeaveManagement.BlazorUI.Services.Base;
using Microsoft.JSInterop;

namespace HRLeaveManagement.BlazorUI.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        private readonly IMapper _mapper;
        public LeaveRequestService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            _mapper = mapper;
        }

        public async Task<Response<Guid>> CreateLeaveRequest(LeaveRequestVM leaveRequest)
        {
            try
            {
                var createLeaveRequestCommand = _mapper.Map<CreateLeaveRequestCommand>(leaveRequest);
                await AddBearerToken();
                await _client.LeaveRequestsPOSTAsync(createLeaveRequestCommand);
                return new Response<Guid>() { Success = true, Message = "Creation Successful" };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public Task<List<LeaveRequestVM>> GetLeaveRequests()
        {
            throw new NotImplementedException();
        }
    }
}
