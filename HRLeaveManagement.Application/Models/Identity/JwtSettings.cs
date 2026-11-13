using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application.Models.Identity
{
    public class JwtSettings
    {
        public string key { get; set; }
        public string Issur { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
