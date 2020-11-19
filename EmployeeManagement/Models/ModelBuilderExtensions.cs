using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mark",
                    Department = Dept.IT,
                    Email = "mark@mycodementor.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Nick",
                    Department = Dept.HR,
                    Email = "nick@mycodementor.com"
                },
                new Employee
                {
                    Id = 3,
                    Name = "Mary",
                    Department = Dept.Payroll,
                    Email = "mary@mycodementor.com"
                }
            );
        }
    }
}
