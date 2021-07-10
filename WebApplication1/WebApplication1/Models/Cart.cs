using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Cart
    {
        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; }
        public string DeliveryAddress { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public double TotalOrderPrice { get; set; }
        public string PaymentStatus { get; set; }


        [ForeignKey("UserID")]
        public RegisteredUser RegisteredUser { get; set; }


    }
}
