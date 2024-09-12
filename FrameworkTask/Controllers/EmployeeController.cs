using FrameworkTask.Models;
using FrameworkTask.Services;
using System;
using System.Web.Mvc;

namespace FrameworkTask.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeService employeeService = new EmployeeService();

        // Index action to list all employees
        public ActionResult Index()
        {
            var employees = employeeService.GetEmployees();
            return View(employees);
        }

        public ActionResult Create()
        {
            var departments = employeeService.GetDepartments(); // Get the department list from the service
            ViewBag.Departments = new SelectList(departments, "DeptId", "DeptName"); // Populate ViewBag
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeService.CreateEmployee(employee);
                return RedirectToAction("Index");
            }

            // Repopulate ViewBag in case of an error
            var departments = employeeService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DeptId", "DeptName");

            return View(employee);
        }



        public ActionResult Update(int id)
        {
            var employee = employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Fetch the departments and pass them to the view
            var departments = employeeService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DeptId", "DeptName", employee.DeptId); // Pass selected DeptId

            return View(employee);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employeeService.UpdateEmployee(employee); // Save the updated employee including DeptId
                return RedirectToAction("Index");
            }

            // Repopulate the departments in case of an error
            var departments = employeeService.GetDepartments();
            ViewBag.Departments = new SelectList(departments, "DeptId", "DeptName", employee.DeptId);

            return View(employee);
        }


        // Delete - GET
        public ActionResult Delete(int id)
        {
            var employee = employeeService.GetEmployeeById(id);
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
            employeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id) {
            var employee = employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            return View(employee);
        }
    }
}
