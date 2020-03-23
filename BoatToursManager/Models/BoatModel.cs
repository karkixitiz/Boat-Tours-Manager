using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BoatToursManager.BL;

namespace BoatToursManager.Models
{
    public class BoatModel
    {
        public int id { get; set; }
        public int popularityId { get; set; }
        public int locationId { get; set; }
        [Required(ErrorMessage = "Please enter boat name")]
        public string name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public int capacity { get; set; }
        [DataType(DataType.Currency)]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public decimal pricePerHour { get; set; }
    
        public List<Location> locationList { get; set; }
        public List<PopularityModel> popularityList { get; set; }
        public List<Boat> boatList { get; set; }
        public Boat boat { get; set; }
        public int seasonId { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-M-yy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime startTime { get; set; }
        
        public DateTime endTime { get; set; }
        public int numPersons { get; set; }
        public string title { get; set; }
        public string imagePath { get; set; }

        public HttpPostedFileBase file { get; set; }
        public string  comment { get; set; }
    }
}