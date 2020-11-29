using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditUserViewModel
    {
        //public EditUserViewModel()
        //{
        //    Claims = new List<string>();
        //    Roles = new List<string>();
        //}
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string Email{ get; set; }
        public string City{ get; set; }
        public List<string> Claims { get; set; } = new List<string>();
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
