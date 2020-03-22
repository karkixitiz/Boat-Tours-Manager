using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class Location : IEquatable<Location>, IDBStorable<DAL.Location>
    {
        public int id { get; private set; } = 0;
        public LatLongCoordinate point { get; set; }
        public string name { get; set; }
        private List<Boat> boats = new List<Boat>();
        private List<BoatRental> boatRentals = new List<BoatRental>();

        public Location(LatLongCoordinate point, string name)
        {
            this.point = point ?? throw new ArgumentNullException(nameof(point));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Location(DAL.Location location)
        {
            this.id = location.id;
            this.point = new LatLongCoordinate(location.LatLongCoordinate);
            this.name = location.name;

            foreach (DAL.Boat boat in location.Boat)
            {
                this.boats.Add(new Boat(boat));

                foreach (DAL.BoatRental rental in boat.BoatRental)
                    this.boatRentals.Add(new BoatRental(rental));
            }
        }

        public bool addBoat(Boat boat)
        {
            if (this.boats.Contains(boat))
                return false;

            DAL.Boat b = MainClass.Instance.db.Boat.Where(v => v.id == boat.id).FirstOrDefault();

            if (b == null && (b = boat.saveInDB()) == null)
                return false;

            b.location_id = this.id;
            MainClass.Instance.db.SaveChanges();

            this.boats.Add(boat);
            return true;
        }

        public bool canRentBoat(Boat boat, DateTime startTime, DateTime endTime, int numPersons)
        {
            //DateTime.Compare(startTime, DateTime.Today) < 0
            if (numPersons <= 0 || boat == null || !this.boats.Contains(boat) ||
                DateTime.Compare(startTime, endTime) >= 0 ||
                numPersons > boat.capacity)
                return false;

            foreach (BoatRental value in this.boatRentals)
            {
                if (value.boat == boat)
                {
                    /* 
                    * Check, whether the time span of the new rental overlaps 
                    * within an existing rental
                    */
                    if (DateTime.Compare(startTime, value.startTime) >= 0 &&
                        DateTime.Compare(startTime, value.endTime) <= 0 ||
                        DateTime.Compare(endTime, value.startTime) >= 0 &&
                        DateTime.Compare(endTime, value.endTime) <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public BoatRental rentBoat(Boat boat, DateTime startTime, DateTime endTime, int numPersons)
        {
            if (!this.canRentBoat(boat, startTime, endTime, numPersons))
                return null;

            BoatRental rental = new BoatRental(startTime, endTime, boat, numPersons);

            if (rental.saveInDB() == null)
                return null;

            this.boatRentals.Add(rental);
            return rental;
        }

        public bool removeBoat(Boat boat)
        {
            DAL.Boat b = MainClass.Instance.db.Boat.Where(v => v.id == boat.id).FirstOrDefault();

            if (b == null)
                return false;

            b.location_id = 0;
            MainClass.Instance.db.SaveChanges();
            return this.boats.Remove(boat);
        }

        public List<Boat> getBoats()
        {
            return new List<Boat>(this.boats);
        }


        public List<BoatRental> getBoatRentals()
        {
            return new List<BoatRental>(this.boatRentals);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Location);
        }

        public bool Equals(Location other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.Location saveInDB()
        {
            DAL.Location entity = null;

            if (this.point.id == 0)
                return null;
            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.Location.Add(new DAL.Location()
                {
                    latLongCoordinateId = this.point.id,
                    name = this.name
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.Location.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.latLongCoordinateId = this.point.id;
                entity.name = this.name;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(Location location1, Location location2)
        {
            return EqualityComparer<Location>.Default.Equals(location1, location2);
        }

        public static bool operator !=(Location location1, Location location2)
        {
            return !(location1 == location2);
        }
    }
}