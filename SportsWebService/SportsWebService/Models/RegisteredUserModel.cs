using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWebService.Models
{
    public class RegisteredUserModel
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password cannot be empty")]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name = "Re-Type Password")]
        [Compare("UserPassword", ErrorMessage = "Password Mismathc")]
        public string RetypePassword { get; set; }
    }
}
