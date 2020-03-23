using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class Ship : IEquatable<Ship>, IDBStorable<DAL.Ship>
    {
        public int id { get; private set; } = 0;
        public int ? capacity { get; set; }
        public string name { get; set; }

        public Ship(int capacity, string name)
        {
            this.capacity = capacity;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Ship(DAL.Ship ship)
        {
            this.id = ship.id;
            this.capacity = ship.capacity;
            this.name = ship.name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Ship);
        }

        public bool Equals(Ship other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.Ship saveInDB()
        {
            DAL.Ship entity = null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.Ship.Add(new DAL.Ship()
                {
                    capacity = this.capacity,
                    name = this.name
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.Ship.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.capacity = this.capacity;
                entity.name = this.name;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(Ship ship1, Ship ship2)
        {
            return EqualityComparer<Ship>.Default.Equals(ship1, ship2);
        }

        public static bool operator !=(Ship ship1, Ship ship2)
        {
            return !(ship1 == ship2);
        }
    }
}