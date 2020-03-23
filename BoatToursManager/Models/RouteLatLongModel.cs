using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class RouteLatLongModel
    {
        public int zIndex { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string title { get; set; }
    }
}