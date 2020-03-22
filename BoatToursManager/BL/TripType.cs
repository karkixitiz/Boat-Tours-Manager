using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class TripType : IEquatable<TripType>, IDBStorable<DAL.TripType>
    {
        public int id { get; private set; } = 0;
        public string name { get; set; }
        public decimal ? driveTimeMultiplier { get; set; }

        public TripType(string name, decimal driveTimeMultiplier)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.driveTimeMultiplier = driveTimeMultiplier;
        }

        public TripType(DAL.TripType tripType)
        {
            this.id = tripType.id;
            this.name = tripType.name;
            this.driveTimeMultiplier = tripType.driveTimeMultiplier;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TripType);
        }

        public bool Equals(TripType other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.TripType saveInDB()
        {
            DAL.TripType entity = null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.TripTypes.Add(new DAL.TripType()
                {
                    name = this.name,
                    driveTimeMultiplier = this.driveTimeMultiplier
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.TripTypes.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.name = this.name;
                entity.driveTimeMultiplier = this.driveTimeMultiplier;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(TripType type1, TripType type2)
        {
            return EqualityComparer<TripType>.Default.Equals(type1, type2);
        }

        public static bool operator !=(TripType type1, TripType type2)
        {
            return !(type1 == type2);
        }
    }
}