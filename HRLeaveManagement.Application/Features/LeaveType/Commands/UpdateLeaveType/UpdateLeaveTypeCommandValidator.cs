using FluentValidation;
using HRLeaveManagement.Application.Contracts.Presistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            RuleFor(p => p.Id)
                .NotNull().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
                .MustAsync(LeaveTypeMustExists)
                .WithMessage("LeaveType not found.");
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

        private async Task<bool> LeaveTypeMustExists(int id, CancellationToken token)
        {
            return await _leaveTypeRepository.IsLeaveTypeExist(id); 

        }

        private async Task<bool> LeaveTypeNameUnique(UpdateLeaveTypeCommand command, CancellationToken token)
        {
            return await _leaveTypeRepository.IsLeaveTypeUnique(command.Name);
        }
    }
}
