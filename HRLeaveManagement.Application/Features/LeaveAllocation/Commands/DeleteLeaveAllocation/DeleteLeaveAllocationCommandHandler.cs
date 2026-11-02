using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Exceptions;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<DeleteLeaveAllocationCommandHandler> _logger;
        public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IAppLogger<DeleteLeaveAllocationCommandHandler> logger)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var leaveAllocationToDelete = await _leaveAllocationRepository.GetByIdAsync(request.Id);
            //Validation: check if the leaveAllocation dose not exists
            if (leaveAllocationToDelete == null)
            {
                _logger.LogError($"The leave allocation wanted to be deleted dose not exist");
                throw new NotFoundException(nameof(LeaveAllocation), request.Id);
            }
            //delete the leave allocation from the database
            await _leaveAllocationRepository.DeleteAsync(leaveAllocationToDelete);

            //logging the successful detetion of the leave allocation
            _logger.LogInformation($"The leave Allocation with Id {leaveAllocationToDelete.Id} was deleted successfully.");

            //return nothing
            return Unit.Value;
        }
    
    }
}
