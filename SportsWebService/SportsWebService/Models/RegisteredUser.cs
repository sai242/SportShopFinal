using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWebService.Models
{
    public class RegisteredUser
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username cannot be empty")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string Username { get; set; }

        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }


        [Required(ErrorMessage = "Name cannot be empty")]
        [RegularExpression("([A-Za-z])*", ErrorMessage = "Must be in proper format")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phonenumber cannot be empty")]
        [RegularExpression("([0-9])*", ErrorMessage = "Must be in proper format")]
        public string Phonenumber { get; set; }

        [Required(ErrorMessage = "DeliveryAddress cannot be empty")]
        public string DeliveryAddress { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
