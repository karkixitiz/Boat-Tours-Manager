using BoatToursManager.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class BoatListViewModel
    {
            public int locationId { get; set; }
            public List<Location> locationList { get; set; }

            public IEnumerable<Boat> boatList { get; set; }
            [Required(ErrorMessage = "Please enter boat name")]
            public string name { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
            public int capacity { get; set; }
            [DataType(DataType.Currency)]
            [Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
            public decimal pricePerHour { get; set; }
        
    }
}