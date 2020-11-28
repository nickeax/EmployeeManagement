using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}
