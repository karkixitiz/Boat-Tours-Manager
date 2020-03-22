using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public class GroupTrip : IEquatable<GroupTrip>, IDBStorable<DAL.GroupTrip>
    {
        public int id { get; private set; } = 0;
        public readonly Route route;
        public DateTime depatureTime { get; }
        public DateTime returnTime { get; }
        private Dictionary<PriceCategory, int> personsOnBoard = new Dictionary<PriceCategory, int>();
        public readonly TripType tripType;
        public readonly Ship ship;

        public GroupTrip(Route route, DateTime depatureTime, TripType tripType, Ship ship)
        {
            this.depatureTime = depatureTime;
            this.returnTime = this.depatureTime.AddMinutes((double)(route.driveTimeMinutes * tripType.driveTimeMultiplier));
            this.route = route ?? throw new ArgumentNullException(nameof(route));
            this.tripType = tripType ?? throw new ArgumentNullException(nameof(tripType));
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));
        }

        public GroupTrip(ScheduledRoute scheduledRoute, Ship ship)
        {
            this.route = scheduledRoute ?? throw new ArgumentNullException(nameof(scheduledRoute));
            this.depatureTime = scheduledRoute.depatureTime;
            this.returnTime = scheduledRoute.getReturnTime();
            this.tripType = scheduledRoute.tripType;
            this.ship = ship ?? throw new ArgumentNullException(nameof(ship));
        }

        public GroupTrip(DAL.GroupTrip groupTrip)
        {
            this.id = groupTrip.id;

            this.route = new Route(groupTrip.Route);
            this.depatureTime = depatureTime;
            this.returnTime = returnTime;
            this.personsOnBoard = personsOnBoard ?? throw new ArgumentNullException(nameof(personsOnBoard));
            this.tripType = new TripType(groupTrip.TripType);
            this.ship = new Ship(groupTrip.Ship);

            foreach (DAL.GroupTripPriceCategory gtpc in groupTrip.GroupTripPriceCategories)
                this.personsOnBoard.Add(new PriceCategory(gtpc.PriceCategory), gtpc.quantity);
        }

        public bool setPersonsOnBoard(PriceCategory priceCategory, int quantity)
        {
            if (priceCategory != null && quantity > 0 && this.getNumPersonsOnBoard() + quantity <= this.ship.capacity)
            {
                this.personsOnBoard.Add(priceCategory, quantity);
                return true;
            }
            return false;
        }

        public bool addPersonOnBoard(PriceCategory priceCategory)
        {
            if (priceCategory != null && this.getNumPersonsOnBoard() + 1 <= this.ship.capacity)
            {
                int value;

                if (this.personsOnBoard.TryGetValue(priceCategory, out value))
                {
                    this.personsOnBoard.Add(priceCategory, value + 1);
                    return true;
                }
            }
            return false;
        }

        public int getPersonsOnBoard(PriceCategory priceCategory)
        {
            int personsOnBoard;

            if (this.personsOnBoard.TryGetValue(priceCategory, out personsOnBoard))
                return personsOnBoard;

            return 0;
        }

        public List<PriceCategory> getPriceCategories()
        {
            return this.personsOnBoard.Keys.ToList();
        }

        public int getNumPersonsOnBoard()
        {
            int totalPersonsOnBoard = 0;

            foreach (KeyValuePair<PriceCategory, int> entry in this.personsOnBoard)
                totalPersonsOnBoard += entry.Value;

            return totalPersonsOnBoard;
        }

        public decimal getTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (KeyValuePair<PriceCategory, int> entry in this.personsOnBoard)
                totalPrice += entry.Key.price * entry.Value;

            return totalPrice;
        }

        public decimal getTotalDriveTimeMinutes()
        {
            return this.route.driveTimeMinutes * this.tripType.driveTimeMultiplier;
        }

        bool isActive()
        {
            DateTime now = DateTime.Now;
            return DateTime.Compare(now, this.depatureTime) >= 0 && DateTime.Compare(now, this.returnTime) <= 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GroupTrip);
        }

        public bool Equals(GroupTrip other)
        {
            return other != null &&
                   this.id == other.id;
        }

        public override int GetHashCode()
        {
            return 1877310944 + this.id.GetHashCode();
        }

        public DAL.GroupTrip saveInDB()
        {
            DAL.GroupTrip entity = null;

            if (this.route.id == 0 || this.ship.id == 0 || this.tripType.id == 0)
                return null;

            // Create, if not existant
            if (this.id == 0)
            {
                entity = MainClass.Instance.db.GroupTrip.Add(new DAL.GroupTrip()
                {
                    startTime = this.depatureTime,
                    endTime = this.returnTime,
                    routeId = this.route.id,
                    shipId = this.ship.id,
                    tripTypeId = this.tripType.id
                });

                foreach (KeyValuePair<PriceCategory, int> entry in this.personsOnBoard)
                {
                    entity.GroupTripPriceCategory.Add(new DAL.GroupTripPriceCategory()
                    {
                        group_trip_id = this.id,
                        price_category_id = entry.Key.id,
                        quantity = entry.Value
                    });
                }
                MainClass.Instance.db.SaveChanges();
                this.id = entity.id;
            }
            else
            {
                entity = MainClass.Instance.db.GroupTrip.Where(v => v.id == this.id).FirstOrDefault();

                if (entity == null)
                    return null;

                entity.startTime = this.depatureTime;
                entity.endTime = this.returnTime;
                entity.routeId = this.route.id;
                entity.shipId = this.ship.id;
                entity.tripTypeId = this.tripType.id;
                MainClass.Instance.db.SaveChanges();
            }
            return entity;
        }

        public static bool operator ==(GroupTrip trip1, GroupTrip trip2)
        {
            return EqualityComparer<GroupTrip>.Default.Equals(trip1, trip2);
        }

        public static bool operator !=(GroupTrip trip1, GroupTrip trip2)
        {
            return !(trip1 == trip2);
        }
    }
}