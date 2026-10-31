using FluentValidation;
using HRLeaveManagement.Application.Contracts.Presistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation
{
    public class UpdateLeaveAllocationCommandValidator : AbstractValidator<UpdateLeaveAllocationCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        public UpdateLeaveAllocationCommandValidator(ILeaveTypeRepository leaveTypeRepository, ILeaveAllocationRepository leaveAllocationRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            //RuleFor LeaveTypeId
            RuleFor(p => p.LeaveTypeId)
               .NotNull().WithMessage("{PropertyName} is required.")
               .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}.")
               .MustAsync(LeaveTypeMustExist)
               .WithMessage("{PropertyName} does not exist.");
            //RuleFor NumberOfDays
            RuleFor(P => P.NumberOfDays)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}.");
            //RuleFor Period
            RuleFor(P => P.Period)
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("{PropertyName} must be after {ComparisonValue}.");
            //RuleFor Id
            RuleFor(P => P.Id)
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(LeaveAllocationMustExist).WithMessage("{PropertyName} does not exist.");
        }

        private async Task<bool> LeaveAllocationMustExist(int id, CancellationToken token)
        {
            return await _leaveAllocationRepository.IsLeaveAllocationExist(id);
        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
        {
            return await _leaveTypeRepository.IsLeaveTypeExist(id);
        }
    }
}
