using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class BoatRental : IEquatable<BoatRental>, IDBStorable<DAL.BoatRental>
    {
        public int id { get; private set; } = 0;
        public DateTime  startTime { get; }
        public DateTime  endTime { get; }
        public Boat boat { get; }
        public int  numPersons { get; }

        public BoatRental(DateTime startTime, DateTime endTime, Boat boat, int numPersons, int id = 0)
        {
            this.boat = boat ?? throw new ArgumentNullException(nameof(boat));

            if (DateTime.Compare(startTime, endTime) >= 0)
                throw new ArgumentException("The startTime cannot be later or equal than the endTime");

            if (numPersons <= 0 || numPersons > boat.capacity)
                throw new ArgumentException("Illegal number of persons. The number of persons " +
                    "has to be orientated on the capacity of the boat to be rented");

            this.startTime = startTime;
            this.endTime = endTime;
            this.numPersons = numPersons;
            this.id = id;
        }

        public BoatRental(DAL.BoatRental boatRental)
        {
            this.id = boatRental.id;
            this.startTime = boatRental.startTime ?? DateTime.Now;
            this.endTime = boatRental.endTime ?? DateTime.Now;
            this.boat = new Boat(boatRental.Boat);
            this.numPersons = boatRental.numPersons ?? 0;
        }

        public decimal getTotalPrice()
        {
            return this.boat.pricePerHour * (int)Math.Ceiling((this.endTime- this.startTime).TotalHours);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BoatRental);
        }

        public bool Equals(BoatRental other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.BoatRental saveInDB()
        {
            DAL.BoatRental entity = null;

            if (this.boat.id == 0) return null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.BoatRental.Add(new DAL.BoatRental()
                {
                    boatId = this.boat.id,
                    numPersons = this.numPersons,
                    startTime = this.startTime,
                    endTime = this.endTime
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.BoatRental.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.boatId = this.boat.id;
                entity.numPersons = this.numPersons;
                entity.startTime = this.startTime;
                entity.endTime = this.endTime;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(BoatRental rental1, BoatRental rental2)
        {
            return EqualityComparer<BoatRental>.Default.Equals(rental1, rental2);
        }

        public static bool operator !=(BoatRental rental1, BoatRental rental2)
        {
            return !(rental1 == rental2);
        }
    }
}