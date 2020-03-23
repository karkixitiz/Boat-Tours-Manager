using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoatToursManager.DAL;

namespace BoatToursManager.BL
{
    public class Address : IEquatable<Address>, IDBStorable<DAL.Address>
    {
        public int id { get; private set; } = 0;
        public string name { get; set; }
        public string streetName { get; set; }
        public string location { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }

        public Address(string name, string streetName, string location, string postalCode, string country)
        {
            this.name = name;
            this.streetName = streetName;
            this.location = location;
            this.postalCode = postalCode;
            this.country = country;
        }

        public Address(DAL.Address address)
        {
            this.id = address.id;
            this.name = address.name ?? throw new ArgumentNullException(nameof(address.name));
            this.streetName = address.streetName ?? throw new ArgumentNullException(nameof(address.streetName));
            this.location = address.location ?? throw new ArgumentNullException(nameof(address.location));
            this.postalCode = address.postCode ?? throw new ArgumentNullException(nameof(address.postCode));
            this.country = address.country ?? throw new ArgumentNullException(nameof(address.country));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Address);
        }

        public bool Equals(Address other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.Address saveInDB()
        {
            DAL.Address entity = null;
            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.Address.Add(new DAL.Address()
                {
                    country = this.country,
                    location = this.location,
                    name = this.name,
                    postCode = this.postalCode,
                    streetName = this.streetName
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.Address.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null) return null;

                entity.country = this.country;
                entity.location = this.location;
                entity.name = this.name;
                entity.postCode = this.postalCode;
                entity.streetName = this.streetName;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }
        public static bool operator ==(Address address1, Address address2)
        {
            return EqualityComparer<Address>.Default.Equals(address1, address2);
        }

        public static bool operator !=(Address address1, Address address2)
        {
            return !(address1 == address2);
        }
    }
}