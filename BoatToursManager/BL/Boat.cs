using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class Boat : IEquatable<Boat>, IDBStorable<DAL.Boat>
    {
        public int id { get; set; } = 0;
        public string name { get; set; }
        public int capacity { get; set; }
        public decimal pricePerHour { get; set; }
        public int seasonId { get; set; }
        public int likeCount { get; set; }
        public int disLikeCount { get; set; }
        public string title { get; set; }
        public string imagePath { get; set; }
        public int locationId { get; set; }

        public Boat(string name, int capacity, decimal pricePerHour, int seasonId, string imagePath)
        {
            this.name = name;
            this.capacity = capacity <= 0 ? 1 : capacity;
            this.pricePerHour = pricePerHour < 0 ? -pricePerHour : pricePerHour;
            this.seasonId = seasonId;
            this.likeCount = 0;
            this.disLikeCount = 0;
            this.title = title;
            this.imagePath = imagePath;
        }
        public Boat()
        {

        }
        public Boat(DAL.Boat boat)
        {
            this.id = boat.id;
            this.name = boat.name ?? throw new ArgumentNullException(nameof(boat.name));
            this.capacity = boat.capacity;
            this.pricePerHour = boat.pricePerHour;
           // this.likeCount = (int)boat;
            //this.disLikeCount = (int)boat.disLikeCount;
            //this.imagePath = boat.imagePath;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Boat);
        }

        public bool Equals(Boat other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.Boat saveInDB()
        {
            DAL.Boat entity = null;
            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.Boat.Add(new DAL.Boat()
                {
                    capacity = this.capacity,
                    name = this.name,
                    pricePerHour = this.pricePerHour,
                    location_id = 0,
                    //seasonId = this.seasonId,
                   // likeCount = 0,
                    //disLikeCount = 0,
                    //imagePath = this.imagePath

                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.Boat.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null) return null;

                entity.capacity = this.capacity;
                entity.name = this.name;
                entity.pricePerHour = this.pricePerHour;
                //entity.seasonId = this.seasonId;
                //entity.likeCount = this.likeCount;
                //entity.disLikeCount = this.disLikeCount;
                //entity.imagePath = this.imagePath;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(Boat boat1, Boat boat2)
        {
            return EqualityComparer<Boat>.Default.Equals(boat1, boat2);
        }

        public static bool operator !=(Boat boat1, Boat boat2)
        {
            return !(boat1 == boat2);
        }
    }
}