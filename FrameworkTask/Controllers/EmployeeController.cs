using FrameworkTask.Models;
using FrameworkTask.Services;
using System;
using System.Web.Mvc;

namespace FrameworkTask.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController()
        {
            _employeeService = new EmployeeService();
        }

        // Index action to list all employees
        public ActionResult Index()
        {
            var employees = _employeeService.GetEmployees();
            return View(employees);
        }

        
        public ActionResult Create()
        {
            // Load departments for the dropdown
            ViewBag.Departments = new SelectList(_employeeService.GetDepartments(), "DeptId", "DeptName");
            return View();
        }

        [HttpPost]
        public JsonResult GetSubDepartments(int deptId)
        {
            var subDepartments = _employeeService.GetSubDepartments(deptId);
            return Json(subDepartments);
        }


        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeService.CreateEmployee(employee);
                return RedirectToAction("Index");
            }

            // Reload departments
            ViewBag.Departments = new SelectList(_employeeService.GetDepartments(), "DeptId", "DeptName");
            return View(employee);
        }



        // GET: Employee/Edit/5
        public ActionResult Update(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Populate the departments
            ViewBag.Departments = new SelectList(_employeeService.GetDepartments(), "DeptId", "DeptName", employee.DeptId);

            // Fetch subdepartments for the employee's department
            ViewBag.SubDepartments = new SelectList(_employeeService.GetSubDepartments(employee.DeptId), "SubDeptId", "SubDeptName", employee.SubDeptId);

            return View(employee);
        }





        //POST: Employee/Edit/5
    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeService.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }

            // Reload departments and subdepartments if model state is invalid
            ViewBag.Departments = new SelectList(_employeeService.GetDepartments(), "DeptId", "DeptName", employee.DeptId);
            ViewBag.SubDepartments = new SelectList(_employeeService.GetSubDepartments(employee.DeptId), "SubDeptId", "SubDeptName", employee.SubDeptId);
            return View(employee);
        }


        // Delete - GET
        public ActionResult Delete(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }

        // Delete - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _employeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id) {
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }
        
    }
}
