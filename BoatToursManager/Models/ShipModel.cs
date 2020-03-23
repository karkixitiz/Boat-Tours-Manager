using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BoatToursManager.Models
{
    public class ShipModel
    {
        public int id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public int capacity { get; set; }
        [Required(ErrorMessage = "Please enter ship name")]
        public string name { get; set; }
    }
}