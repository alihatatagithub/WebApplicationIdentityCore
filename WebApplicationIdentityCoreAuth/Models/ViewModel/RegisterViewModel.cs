using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationIdentityCoreAuth.Utilities;

namespace WebApplicationIdentityCoreAuth.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", "Account")]
        //[ValidEmailDomainAttribute(allowDomain: "@gmail.com", ErrorMessage = "Email Must @gmail.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]

        [Compare("Password", ErrorMessage = "Not Matched")]

        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
