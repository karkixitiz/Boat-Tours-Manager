using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BoatToursManager.Models;

namespace BoatToursManager.Models
{
    public class LocationModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter location name")]
        public string name { get; set; }

        public List<LatLongModel> point { get; set; }
    }
}