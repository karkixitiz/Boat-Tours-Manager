using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BoatToursManager
{
    public static class CryptoPass
    {
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
    public static class DropDownListUtility
    {
        public static IEnumerable<SelectListItem> GetSeasonDropDown(object selectedValue)
        {
            return new List<SelectListItem>
        {
            new SelectListItem{ Text="Season- Summer", Value = "1", Selected = "1" == selectedValue.ToString()},
            new SelectListItem{ Text="Season- Winter", Value = "2", Selected = "2" == selectedValue.ToString()}
        };
        }
    }
}