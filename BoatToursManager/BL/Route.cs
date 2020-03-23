using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class Route : IEquatable<Route>, IDBStorable<DAL.Route>
    {
        public int id { get; protected set; } = 0;
        public LatLongCoordinate startPoint { get; set; }
        public LatLongCoordinate endPoint { get; set; }
        public decimal driveTimeMinutes { get; set; }
        public int schedulePlanId { get; set; }


        public Route(LatLongCoordinate startPoint, LatLongCoordinate endPoint, decimal driveTimeMinutes)
        {
            this.startPoint = startPoint ?? throw new ArgumentNullException(nameof(startPoint));
            this.endPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            this.driveTimeMinutes = driveTimeMinutes > 0 ? driveTimeMinutes : 0;
        }

        public Route(DAL.Route route)
        {
            this.id = route.id;
            this.startPoint = new LatLongCoordinate(route.LatLongCoordinate1);
            this.endPoint = new LatLongCoordinate(route.LatLongCoordinate);
            this.driveTimeMinutes = route.driveTime;
        }

        public string getRouteString()
        {
            return startPoint.name + " --> " + endPoint.name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Route);
        }

        public bool Equals(Route other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.Route saveInDB()
        {
            DAL.Route entity = null;

            if (this.endPoint.id == 0 || this.startPoint.id == 0)
                return null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.Route.Add(new DAL.Route()
                {
                    driveTime = this.driveTimeMinutes,
                    endPointLatLengId = this.endPoint.id,
                    startPointLatLengId = this.startPoint.id,
                });
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.Route.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.driveTime = this.driveTimeMinutes;
                entity.endPointLatLengId = this.endPoint.id;
                entity.startPointLatLengId = this.startPoint.id;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(Route route1, Route route2)
        {
            return EqualityComparer<Route>.Default.Equals(route1, route2);
        }

        public static bool operator !=(Route route1, Route route2)
        {
            return !(route1 == route2);
        }
    }
}