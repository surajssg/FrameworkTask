using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrameworkTask.Models
{
    public class SubDepartment
    {
        [Key]
        public int SubDeptId { get; set; }
        public string SubDepartmentName { get; set; }
        public int DeptId { get; set; }


    }
}