using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationIdentityCoreAuth.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private string _allowDomain;

        public ValidEmailDomainAttribute(string allowDomain)
        {
            _allowDomain = allowDomain;
                
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
           return strings[1].ToUpper() == _allowDomain.ToUpper();
        }
    }
}
