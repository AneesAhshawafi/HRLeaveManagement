using FluentValidation;
using HRLeaveManagement.Application.Contracts.Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandValidator: AbstractValidator<CreateLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public CreateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {

            RuleFor(P => P.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(70).WithMessage("{PropertyName} must not exceed 70 characters.");
            RuleFor(P => P.DefaultDays)
                .GreaterThan(1).WithMessage("{PropertyName} must be greater than 1.")
                .LessThan(100).WithMessage("{PropertyName} must be less than 100.");
            RuleFor(P => P)
                .MustAsync(LeaveTypeNameUnique)
                .WithMessage("LeaveType is already exists.");
            this._leaveTypeRepository = leaveTypeRepository;

        }

        private async Task<bool> LeaveTypeNameUnique(CreateLeaveTypeCommand command, CancellationToken token)
        {
            return await _leaveTypeRepository.IsLeaveTypeUnique(command.Name);
        }
    }
}
