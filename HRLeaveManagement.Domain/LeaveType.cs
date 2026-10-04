using ClassLibrary1HRLeaveManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1HRLeaveManagement.Domain
{
    public class LeaveType : BaseEntity
    {
        // Validation attributes
        [StringLength(50)]  
        public String Name { get; set; }= String.Empty;
        public int DefaultDays { get; set; }
    }
    

}
