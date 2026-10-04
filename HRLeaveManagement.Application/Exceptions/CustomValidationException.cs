using FluentValidation.Results;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
       
        public CustomValidationException(String message)
            : base(message)
        {
            ValidationErrors = new();
        }
        public CustomValidationException(String message, ValidationResult validationResult)
            : base(message)
        {
            ValidationErrors = new();
            foreach(var error in validationResult.Errors)
            {
                ValidationErrors.Add(error.ErrorMessage);
            }
        }
        public List<String> ValidationErrors { get; set; }
    }
}
