using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class AddressModel
    {
        public string addressName { get; set; }
        [Required(ErrorMessage = "Please enter street name")]
        public string streetName { get; set; }
        [Required(ErrorMessage = "Please enter location name")]
        public string location { get; set; }
        [Required(ErrorMessage = "Please enter postal code")]
        public string postalCode { get; set; }
        [Required(ErrorMessage = "Please enter country name")]
        public string country { get; set; }
    }
}