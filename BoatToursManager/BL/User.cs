using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoatToursManager.DAL;

namespace BoatToursManager.BL
{
    public class User : IEquatable<User>, IDBStorable<DAL.User>
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

        public User(string name, string email, string password, string phone, UserType userType, Address userAddress)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            this.phone = phone;
            this.userType = userType;
            this.userAddress = userAddress;
        }

        public User(BoatToursManager.DAL.User user)
        {
            this.id = user.id;
            this.name = user.name;
            this.email = user.email;
            this.password = user.password;
            this.phone = user.phone;
            this.userType = (UserType)Enum.ToObject(typeof(UserType), user.userType);
          //  this.userAddress = new Address(user.Add);
           // this.activationCode = user.activationCode;
           // this.isEmailVerified = user.isEmailVerified;
            //this.passwordResetCode = user.passwordResetCode;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.User saveInDB()
        {
            if (this.userAddress.id == 0)
                return null;

            DAL.User entity = null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.User.Add(new DAL.User()
                {
                    activationCode = this.activationCode,
                    addressId = this.userAddress.id,
                    email = this.email,
                    isEmailVerified = this.isEmailVerified,
                    name = this.name,
                    password = this.password,
                    phone = this.phone,
                    userType = (this.userType).ToString(),
                    passwordResetCode = this.passwordResetCode
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.User.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.activationCode = this.activationCode;
                entity.addressId = this.userAddress.id;
                entity.email = this.email;
                entity.isEmailVerified = this.isEmailVerified;
                entity.name = this.name;
                entity.password = this.password;
                entity.phone = this.phone;
                entity.userType = (this.userType).ToString();
                entity.passwordResetCode = this.passwordResetCode;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(User user1, User user2)
        {
            return EqualityComparer<User>.Default.Equals(user1, user2);
        }

        public static bool operator !=(User user1, User user2)
        {
            return !(user1 == user2);
        }
    }
}