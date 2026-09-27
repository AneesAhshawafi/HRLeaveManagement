using AutoMapper;
using HRLeaveManagement.Application.Contracts.Presistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }
        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // Validate icoming data.

            // Convert to domain entity object.
            var leaveTypeToUpdate = _mapper.Map<ClassLibrary1HRLeaveManagement.Domain.LeaveType>(request);
            // Update the data in the database.
            await _leaveTypeRepository.UpdateAsync(leaveTypeToUpdate);

            //return the unit value to indicate that the command has been handled successfully.
            return Unit.Value;

        }
    }
}
