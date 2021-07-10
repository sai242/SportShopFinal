using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWebService.Models
{
    public class User
    {
        [Required(ErrorMessage = "Username cannot be empty")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
