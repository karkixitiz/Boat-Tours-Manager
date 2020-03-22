using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class SchedulePlan : IEquatable<SchedulePlan>, IDBStorable<DAL.SchedulePlan>
    {
            public int id { get; private set; } = 0;
            public DateTime ? beginDate { get; set; }
            public DateTime ? endDate { get; set; }
            private List<ScheduledRoute> routes = new List<ScheduledRoute>();

            public SchedulePlan(DateTime beginDate, DateTime endDate)
            {
                this.beginDate = beginDate;
                this.endDate = endDate;
            }
            public SchedulePlan(DAL.SchedulePlan schedulePlan)
            {
                this.id = schedulePlan.id;
                this.beginDate = schedulePlan.beginDate;
                this.endDate = schedulePlan.endDate;

                foreach (DAL.ScheduleRoute scheduledRoute in schedulePlan.ScheduleRoute)
                    this.routes.Add(new ScheduledRoute(scheduledRoute.Route, scheduledRoute));
            }

            public bool addScheduledRoute(ScheduledRoute route)
            {
                if (this.routes.Contains(route))
                    return false;

                DateTime returnTime = route.getReturnTime();

                if (DateTime.Compare(route.depatureTime, beginDate ?? DateTime.Now) >= 0 &&
                    DateTime.Compare(route.depatureTime, endDate ?? DateTime.Now) <= 0 &&
                    DateTime.Compare(returnTime, endDate ?? DateTime.Now) <= 0)
                {
                    DAL.ScheduleRoute entity = MainClass.Instance.db.ScheduledRoutes.Where(v => v.routeId == route.id).FirstOrDefault();

                    if (entity == null && (entity = route.saveInDB()) == null)
                        return false;

                    entity.shedule_plan_id = this.id;
                    MainClass.Instance.db.SaveChanges();
                    this.routes.Add(route);
                    return true;
                }
                return false;
            }

            public bool removeScheduledRoute(ScheduledRoute route)
            {
                DAL.ScheduleRoute entity = MainClass.Instance.db.ScheduledRoutes.Where(v => v.routeId == route.id).FirstOrDefault();

                if (entity == null)
                    return false;

                entity.shedule_plan_id = 0;
                MainClass.Instance.db.SaveChanges();
                return this.routes.Remove(route);
            }

            public List<ScheduledRoute> getScheduledRoutes()
            {
                return new List<ScheduledRoute>(this.routes);
            }

            bool isActive()
            {
                DateTime now = DateTime.Now;
                return DateTime.Compare(now, beginDate ?? DateTime.Now) >= 0 && DateTime.Compare(now, endDate ?? DateTime.Now) <= 0;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as SchedulePlan);
            }

            public bool Equals(SchedulePlan other)
            {
                return other != null &&
                       this.id == other.id;
            }

            public override int GetHashCode()
            {
                return 1877310944 + this.id.GetHashCode();
            }

            public DAL.SchedulePlan saveInDB()
            {
                DAL.SchedulePlan entity = null;

                // Create, if not existant
                if (this.id == 0)
                {
                    entity = MainClass.Instance.db.SchedulePlans.Add(new DAL.SchedulePlan()
                    {
                        beginDate = this.beginDate,
                        endDate = this.endDate
                    });
                    MainClass.Instance.db.SaveChanges();
                    this.id = entity.id;
                }
                else
                {
                    entity = MainClass.Instance.db.SchedulePlans.Where(v => v.id == this.id).FirstOrDefault();

                    if (entity == null)
                        return null;

                    entity.beginDate = this.beginDate;
                    entity.endDate = this.endDate;
                    MainClass.Instance.db.SaveChanges();
                }
                return entity;
            }

            public static bool operator ==(SchedulePlan plan1, SchedulePlan plan2)
            {
                return EqualityComparer<SchedulePlan>.Default.Equals(plan1, plan2);
            }

            public static bool operator !=(SchedulePlan plan1, SchedulePlan plan2)
            {
                return !(plan1 == plan2);
            }
        }
}