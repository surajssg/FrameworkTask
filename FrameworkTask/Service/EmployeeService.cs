using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FrameworkTask.Models;

namespace FrameworkTask.Services
{
    public class EmployeeService
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllEmployees", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeID = reader["EmployeeID"] != DBNull.Value ? Convert.ToInt32(reader["EmployeeID"]) : 0,
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            Gender = reader["Gender"].ToString(),
                            DesignationID = reader["DesignationID"] != DBNull.Value ? Convert.ToInt32(reader["DesignationID"]) : 0,
                            ContactNo = reader["ContactNo"].ToString(),
                            DeptId = reader["DeptId"] != DBNull.Value ? Convert.ToInt32(reader["DeptId"]) : 0
                        };

                        employees.Add(employee);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching employees", ex);
                }
            }

            return employees;
        }


        // Create a new employee
        public void CreateEmployee(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCreateEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@DOB", (object)employee.DOB ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DesignationID", employee.DesignationID);
                cmd.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                cmd.Parameters.AddWithValue("@DeptId", employee.DeptId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating employee", ex);
                }
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeID);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB == default(DateTime) ? (object)DBNull.Value : employee.DOB);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DesignationID", employee.DesignationID);
                cmd.Parameters.AddWithValue("@ContactNo", employee.ContactNo);
                cmd.Parameters.AddWithValue("@DeptId", employee.DeptId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("No rows were updated. Please check if the EmployeeID exists.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating employee", ex);
                }
            }
        }


        // Delete employee by ID
        public void DeleteEmployee(int employeeId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error deleting employee", ex);
                }
            }
        }

        // Get employee by ID
        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployeeById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            EmployeeID = reader["EmployeeID"] != DBNull.Value ? Convert.ToInt32(reader["EmployeeID"]) : 0,
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            Gender = reader["Gender"].ToString(),
                            DesignationID = reader["DesignationID"] != DBNull.Value ? Convert.ToInt32(reader["DesignationID"]) : 0,
                            ContactNo = reader["ContactNo"].ToString(),
                            DeptId = reader["DeptId"] != DBNull.Value ? Convert.ToInt32(reader["DeptId"]) : 0
                        };
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching employee by ID", ex);
                }
            }

            return employee;
        }

        // Fetch list of departments
        public List<Department> GetDepartments()
        {
            List<Department> department= new List<Department>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DeptId, DeptName FROM Department"; // Ensure 'Departments' table name is correct
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        department.Add(new Department
                        {
                            DeptId = reader["DeptId"] != DBNull.Value ? Convert.ToInt32(reader["DeptId"]) : 0,
                            DeptName = reader["DeptName"].ToString()
                        });
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching departments", ex);
                }
            }

            return department;
        }

        // Fetch list of subdepartments by department ID
        public List<SubDepartment> GetSubDepartments(int deptId)
        {
            List<SubDepartment> subDepartments = new List<SubDepartment>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SubDeptId, SubDeptName FROM SubDepartments WHERE DeptId = @DeptId"; // Ensure 'SubDepartments' table name is correct
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DeptId", deptId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        subDepartments.Add(new SubDepartment
                        {
                            SubDeptId = reader["SubDeptId"] != DBNull.Value ? Convert.ToInt32(reader["SubDeptId"]) : 0,
                            SubDepartmentName = reader["SubDeptName"].ToString()
                        });
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching subdepartments", ex);
                }
            }

            return subDepartments;
        }
    }
}
