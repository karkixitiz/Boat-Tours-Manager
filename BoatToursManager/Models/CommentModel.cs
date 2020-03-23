using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class CommentModel
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int userId { get; set; }
        public int boatId { get; set; }
        public DateTime commentDate { get; set; }
    }
}