using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class OrderModel
    {
        public int orderNumber { get; set; }
        public DateTime orderDate { get; set; }
        public string userName { get; set; }
        public decimal price { get; set; }
        public string paymentType { get; set; }
        public string orderType { get; set; }
    }
}