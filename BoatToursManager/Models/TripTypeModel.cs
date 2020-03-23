using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BoatToursManager.Models
{
    public class TripTypeModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter trip type name")]
        public string name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public decimal driveTimeMultiplier { get; set; }
    }
}