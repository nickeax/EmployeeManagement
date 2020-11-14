using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employees;
        public MockEmployeeRepository()
        {
            _employees = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@nfp.com" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT, Email = "john@nfp.com" },
                new Employee() { Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam@nfp.com" }
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> temp = _employees;
            return temp;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employees.Max(i => i.Id) + 1;
            _employees.Add(employee);

            return employee;
        }

        public Employee GetEmployee(int Id)
        {
            return _employees.FirstOrDefault(x => x.Id == Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee tmp = _employees.FirstOrDefault(e => e.Id == employeeChanges.Id);

            if (tmp != null)
            {
                tmp.Name = employeeChanges.Name;
                tmp.Email = employeeChanges.Email;
                tmp.Department = employeeChanges.Department;
            }

            return tmp;
        }

        public Employee Delete(int id)
        {
            Employee tmp = _employees.FirstOrDefault(e => e.Id == id);

            if(tmp != null)
            {
                _employees.Remove(tmp);
            }

            return tmp;
        }
    }
}
