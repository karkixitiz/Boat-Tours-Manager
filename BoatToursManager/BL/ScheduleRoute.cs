using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoatToursManager.DAL;

namespace BoatToursManager.BL
{
    public class ScheduledRoute : Route, IDBStorable<DAL.ScheduleRoute>
    {
        public DateTime depatureTime { get; set; }
        public TripType tripType { get; set; }

        public ScheduledRoute(LatLongCoordinate startPoint, LatLongCoordinate endPoint, decimal driveTimeMinutes,
            DateTime depatureTime, TripType tripType) : base(startPoint, endPoint, driveTimeMinutes)
        {
            this.depatureTime = depatureTime;
            this.tripType = tripType ?? throw new ArgumentNullException(nameof(tripType));
        }

        public ScheduledRoute(DAL.Route route, DAL.ScheduleRoute scheduledRoute) : base(route)
        {
            this.depatureTime = scheduledRoute.depatureTime;
            this.tripType = new TripType(scheduledRoute.TripType);
        }

        public DateTime getReturnTime()
        {
            return this.depatureTime.AddMinutes((double)(this.driveTimeMinutes * this.tripType.driveTimeMultiplier));
        }

        public new DAL.ScheduleRoute saveInDB()
        {
            if (this.tripType.id == 0)
                return null;

            DAL.Route route = base.saveInDB();

            if (route == null)
                return null;

            if (route.ScheduleRoute != null)
            {
                route.ScheduleRoute.depatureTime = this.depatureTime;
                route.ScheduleRoute.tripTypeId = this.tripType.id;
                route.ScheduleRoute.shedule_plan_id = 0;
            }
            else
            {
                route.ScheduleRoute = new DAL.ScheduleRoute()
                {
                    depatureTime = this.depatureTime,
                    tripTypeId = this.tripType.id,
                    routeId = route.id,
                    shedule_plan_id = 0
                };
            }
            MainClass.Instance.db.SaveChanges();
            return route.ScheduleRoute;
        }

        ScheduleRoute IDBStorable<ScheduleRoute>.saveInDB()
        {
            throw new NotImplementedException();
        }
    }
}