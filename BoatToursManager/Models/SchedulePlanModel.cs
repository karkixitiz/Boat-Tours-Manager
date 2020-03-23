using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class SchedulePlanModel
    {
        public int id { get; private set; } = 0;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-M-yy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime beginDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-M-yy hh:mm:TT}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }
    }
}