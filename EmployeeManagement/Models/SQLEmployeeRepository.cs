using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbc;

        public SQLEmployeeRepository(AppDbContext dbc)
        {
            this._dbc = dbc;
        }
        public Employee Add(Employee employee)
        {
            _dbc.Add(employee);
            _dbc.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee tmp = _dbc.Employees.Find(id);
            if(tmp != null)
            {
                _dbc.Employees.Remove(tmp);
                _dbc.SaveChanges();
            }
            return tmp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _dbc.Employees;
        }

        public Employee GetEmployee(int Id)
        {
            return _dbc.Employees.Find(Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _dbc.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbc.SaveChanges();

            return employeeChanges;
        }
    }
}
