using BoatToursManager.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class PriceCategoryModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public TripType tripType { get; set; }
        public PersonCategory personCategory { get; set; }
        public decimal price { get; set; }
        public Route route { get; set; }
    }
}