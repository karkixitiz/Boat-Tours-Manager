using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class LatLongCoordinate : IEquatable<LatLongCoordinate>, IDBStorable<DAL.LatLongCoordinate>
    {
        public int id { get; private set; } = 0;
        public decimal ? latitude { get; set; }
        public decimal ? longitude { get; set; }
        public string name { get; set; }

        public LatLongCoordinate(decimal latititude, decimal longitude, string name)
        {
            this.latitude = latititude;
            this.longitude = longitude;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public LatLongCoordinate(DAL.LatLongCoordinate latLongCoordinate)
        {
            this.id = latLongCoordinate.id;
            this.latitude = latLongCoordinate.latitude;
            this.longitude = latLongCoordinate.longitude;
            this.name = latLongCoordinate.name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LatLongCoordinate);
        }

        public bool Equals(LatLongCoordinate other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.LatLongCoordinate saveInDB()
        {
            DAL.LatLongCoordinate entity = null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.LatLongCoordinates.Add(new DAL.LatLongCoordinate()
                {
                    latitude = this.latitude,
                    longitude = this.longitude,
                    name = this.name
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.LatLongCoordinates.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.latitude = this.latitude;
                entity.longitude = this.longitude;
                entity.name = this.name;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(LatLongCoordinate coordinate1, LatLongCoordinate coordinate2)
        {
            return EqualityComparer<LatLongCoordinate>.Default.Equals(coordinate1, coordinate2);
        }

        public static bool operator !=(LatLongCoordinate coordinate1, LatLongCoordinate coordinate2)
        {
            return !(coordinate1 == coordinate2);
        }
    }
}