using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name,object key ):base($"{name} ({key}) was not found")
        {
        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
