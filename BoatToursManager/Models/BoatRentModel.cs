using BoatToursManager.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoatToursManager.Models
{
    public class BoatRentModel
    {
        public int locationId { get; set; }
        public string locationName { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime startTime { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime endTime { get; set; }
        public Boat boat { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public int numPersons { get; set; }
        public List<Location> locationList { get; set; }
        public List<Boat> boatList { get; set; }
        public int boatId { get; set; }
        public decimal totalPrice { get; set; }
        


    }
}