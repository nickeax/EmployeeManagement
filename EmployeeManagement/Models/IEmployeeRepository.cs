﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        Employee Add(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
    }
}
