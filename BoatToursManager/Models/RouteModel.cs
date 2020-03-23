using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BoatToursManager.Models
{
    public class RouteModel
    {
        public int id { get; set; }
        public List<RouteLatLongModel> pointList { get; set; }
        [Required(ErrorMessage = "Please enter a name for the start point")]
        public string startPointName { get; set; }
        [Required(ErrorMessage = "Please enter a name for the end point")]
        public string endPointName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public decimal driveTimeMinutes { get; set; }
        public string name { get; set; }
    }
}