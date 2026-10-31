using FluentValidation;
using HRLeaveManagement.Application.Contracts.Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandValidator : AbstractValidator<CreateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public CreateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            RuleFor(p => p.LeaveTypeId)
                .NotNull().WithMessage("{PropertyName} must be present.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}.")
                .MustAsync(LeaveTypeMustExist).WithMessage("{PropertyName} does not exist.");
            _leaveTypeRepository = leaveTypeRepository;
        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
        {
            return await _leaveTypeRepository.IsLeaveTypeExist(id);
        }
    }
}
