using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class LoginDataVM
    {
        [Required]
        public string Email { get; set; }

        public string RoleName { get; set; }
    }
}
