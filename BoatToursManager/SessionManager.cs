using BoatToursManager.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager
{
    public static class SessionManager
    {
        public static bool checkCurrentUserType(UserType userType)
        {
            User u = (User)System.Web.HttpContext.Current.Session["user"];
            return u != null && u.userType == userType;
        }

        public static bool userIsLoggedIn()
        {
            return ((User)System.Web.HttpContext.Current.Session["user"]) != null;
        }
    }
}