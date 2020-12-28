using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationIdentityCoreAuth.Models.ViewModel
{
    public class EmployeeCreateViewModel
    {

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public IFormFile Photo { get; set; }
        //public List<IFormFile> Photos { get; set; }

    }
}
