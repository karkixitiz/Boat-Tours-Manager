using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class Address
    {
        public int id { get; private set; } = 0;
        public string name { get; set; }
        public string streetName { get; set; }
        public string location { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        
    }
}