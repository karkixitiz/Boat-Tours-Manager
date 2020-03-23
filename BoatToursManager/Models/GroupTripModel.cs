using BoatToursManager.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace BoatToursManager.Models
{
    public class GroupTripModel
    {
        public int id { get; set; }
        public BL.Route route { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-M-yy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime depatureTime { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-M-yy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime returnTime { get; set; }
        public TripType tripType { get; set; }
        public Ship ship { get; set; }
        public List<TripType> tripList { get; set; }
        public List<Ship> shipList { get; set; }
        public List<PriceCategory> priceCategories { get; set; }
        public List<RouteModel> routeList { get; set; }
        public int shipTypeId { get; set; }
        public int tripTypeId { get; set; }
        public int routeId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public int adults { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal or greater than {1}")]
        public int children { get; set; }
        public decimal totalPrice { get; set; }
        public string routeName { get; set; }
        public decimal adultPrice { get; set; }
        public decimal childPrice { get; set; }
        public int driveTimeMinutes { get; set; }

        public BL.GroupTrip finalGroupTrip;
    }
}