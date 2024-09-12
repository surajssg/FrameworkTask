using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameworkTask.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public int DesignationID { get; set; }
        public string ContactNo { get; set; }
        public string Language { get; set; }

        public int DeptId { get; set; }

    }
}