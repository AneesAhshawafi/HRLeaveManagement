using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;

public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand>
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IAppLogger<DeleteLeaveRequestCommandHandler> _logger;

    public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,IAppLogger<DeleteLeaveRequestCommandHandler> logger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

        if (leaveRequest == null)
        {
            
            _logger.LogError($"Leave request with ID {request.Id} not found.");
            throw new NotFoundException(nameof(LeaveRequest), request.Id);
        }

        await _leaveRequestRepository.DeleteAsync(leaveRequest);
        _logger.LogInformation($"LeaveRequest {leaveRequest.Id} is successfully deleted.");
        return Unit.Value;
    }
}
