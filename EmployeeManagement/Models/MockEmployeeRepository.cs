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
                new Employee() { Id = 1, Name = "Mary", Department = "HR", Email = "mary@nfp.com" },
                new Employee() { Id = 2, Name = "John", Department = "IT", Email = "john@nfp.com" },
                new Employee() { Id = 3, Name = "Sam", Department = "IT", Email = "sam@nfp.com" }
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> temp = _employees;
            return temp;
        }

        public Employee GetEmployee(int Id)
        {
            return _employees.FirstOrDefault(x => x.Id == Id);
        }
    }
}
