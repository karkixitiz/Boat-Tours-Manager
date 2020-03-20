using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class User
    {
        public int id { get; private set; } = 0;
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public UserType userType { get; set; }
        public Address userAddress { get; set; }
        public Guid activationCode { get; set; } = Guid.NewGuid();
        public bool isEmailVerified { get; set; }
        public Guid passwordResetCode { get; set; }

       
    }
}