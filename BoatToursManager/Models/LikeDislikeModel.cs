using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class LikeDislikeModel
    {
        public int boatId { get; set; }
        public bool status { get; set; }
        public int userId { get; set; }
    }
}