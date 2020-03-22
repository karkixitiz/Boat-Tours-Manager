using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoatToursManager.BL
{
    public sealed class MainClass
    {

        public int id { get; private set; }
        private static MainClass instance = null;
        private int numOrders = 0;
        private List<SchedulePlan> schedulePlans = new List<SchedulePlan>();
        private Dictionary<int, Order> orders = new Dictionary<int, Order>();
        private List<Ship> ships = new List<Ship>();
        private List<Location> locations = new List<Location>();
        private List<Boat> boats = new List<Boat>();
        private List<Route> routes = new List<Route>();
        private List<PersonCategory> personCategories = new List<PersonCategory>();
        private List<PriceCategory> priceCategories = new List<PriceCategory>();
        private List<User> users = new List<User>();
        private List<TripType> tripTypes = new List<TripType>();
        public DAL.BoatTourManagerEntities db { get; } = new DAL.BoatTourManagerEntities();
        private DAL.BoatToursManager entity { get; }

        private MainClass()
        {
            entity = (from manager in db.BoatToursManager select manager).FirstOrDefault();

            if (entity == null)
            {
                entity = db.BoatToursManager.Add(new DAL.BoatToursManager() { id = 1 });
                db.SaveChanges();
            }
            foreach (DAL.Location value in entity.Locations)
                this.locations.Add(new Location(value));
            foreach (DAL.Boat value in db.Boat)
                this.boats.Add(new Boat(value));

            foreach (DAL.PersonCategory value in entity.PersonCategories)
                this.personCategories.Add(new PersonCategory(value));

            foreach (DAL.PriceCategory value in entity.PriceCategories)
                this.priceCategories.Add(new PriceCategory(value));

            foreach (DAL.Route value in entity.Routes)
                this.routes.Add(new Route(value));

            foreach (DAL.SchedulePlan value in entity.SchedulePlans)
                this.schedulePlans.Add(new SchedulePlan(value));

            foreach (DAL.Ship value in entity.Ships)
                this.ships.Add(new Ship(value));

            foreach (DAL.TripType value in entity.TripTypes)
                this.tripTypes.Add(new TripType(value));

            foreach (DAL.User value in entity.Users)
                this.users.Add(new User());

            foreach (DAL.Order value in entity.Orders)
            {
                if (value.GroupTrip != null)
                    this.orders.Add(value.orderNumber??0, new GroupTripOrder(value));
                else if (value.BoatRental != null)
                    this.orders.Add(value.orderNumber??0, new BoatRentalOrder(value));
            }
            this.numOrders = entity.numOrders;
            this.id = entity.id;
        }

        public static MainClass Instance
        {

            get
            {
                if (instance == null)
                    // var db = new DAL.BoatToursManagerEntities();
                    instance = new MainClass();
                return instance;
            }
        }

        public bool canOrderGroupTrip(GroupTrip groupTrip)
        {
            if (groupTrip == null)
                return false;

            if (DateTime.Compare(groupTrip.depatureTime, DateTime.Today) < 0)
                return false;

            if (groupTrip.getNumPersonsOnBoard() <= 0)
                return false;

            List<PriceCategory> priceCategories = groupTrip.getPriceCategories();
            int priceCategoriesFound = 0;

            foreach (PriceCategory value in priceCategories)
                if (this.priceCategories.Contains(value))
                    priceCategoriesFound++;

            if (priceCategoriesFound != priceCategories.Count)
                return false;

            if (!this.routes.Contains(groupTrip.route) || !this.ships.Contains(groupTrip.ship) ||
                !this.tripTypes.Contains(groupTrip.tripType))
                return false;

            foreach (KeyValuePair<int, Order> entry in this.orders)
            {
                GroupTripOrder order = entry.Value as GroupTripOrder;

                if (order != null && groupTrip.ship == order.groupTrip.ship)
                {
                    /* 
                     * Check, whether the time span of the new group trip overlaps with the available group trips 
                     * if it's the same ship 
                     */
                    if (DateTime.Compare(groupTrip.depatureTime, DateTime.Today) < 0 &&
                        DateTime.Compare(groupTrip.depatureTime, order.groupTrip.depatureTime) >= 0 &&
                        DateTime.Compare(groupTrip.depatureTime, order.groupTrip.returnTime) <= 0 ||
                        DateTime.Compare(groupTrip.returnTime, order.groupTrip.depatureTime) >= 0 &&
                        DateTime.Compare(groupTrip.returnTime, order.groupTrip.returnTime) <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public GroupTripOrder orderGroupTrip(GroupTrip groupTrip, PaymentType paymentType, Address orderAddress, User user)
        {
            if (!this.users.Contains(user))
                return null;

            if (groupTrip == null || orderAddress == null || user == null || !this.canOrderGroupTrip(groupTrip))
                return null;

            if (groupTrip.saveInDB() == null)
                return null;

            GroupTripOrder groupTripOrder = new GroupTripOrder(paymentType, DateTime.Now, user, orderAddress, ++numOrders, groupTrip);
            DAL.Order value = groupTripOrder.saveInDB();

            if (value == null)
                return null;

           // value.boatToursManagerId = 1;
            entity.numOrders = this.numOrders;
            db.SaveChanges();

            this.orders.Add(groupTripOrder.orderNumber, groupTripOrder);
            return groupTripOrder;
        }

        public BoatRentalOrder orderBoatRental(BoatRental boatRental, PaymentType paymentType, Address orderAddress, User user)
        {
            if (boatRental == null || orderAddress == null || user == null)
                return null;

            if (!this.users.Contains(user))
                return null;

            bool boatFound = false;
            Location location = null;

            foreach (Location l in this.locations)
            {
                if (l.getBoats().Contains(boatRental.boat))
                {
                    boatFound = true;
                    location = l;
                    break;
                }
            }
            if (!boatFound)
                return null;

            if (!location.getBoatRentals().Contains(boatRental))
                return null;

            BoatRentalOrder boatRentalOrder = new BoatRentalOrder(paymentType, DateTime.Now, user, orderAddress, ++numOrders, boatRental);
            DAL.Order value = boatRentalOrder.saveInDB();

            if (value == null)
                return null;

            //value.boatToursManagerId = 1;
            entity.numOrders = this.numOrders;
            db.SaveChanges();

            this.orders.Add(boatRentalOrder.orderNumber, boatRentalOrder);
            return boatRentalOrder;
        }

        public BoatRentalOrder getBoatRentalOrder(int orderNumber)
        {
            Order order;

            if (this.orders.TryGetValue(orderNumber, out order) && order is BoatRentalOrder)
                return order as BoatRentalOrder;

            return null;
        }

        public GroupTrip getGroupTrip(int orderNumber)
        {
            GroupTripOrder order = this.getGroupTripOrder(orderNumber);
            return order != null ? order.groupTrip : null;
        }

        public BoatRental getBoatRental(int orderNumber)
        {
            BoatRentalOrder order = this.getBoatRentalOrder(orderNumber);
            return order != null ? order.boatRental : null;
        }

        public List<BoatRental> getBoatRentals()
        {
            List<BoatRental> boatRentals = new List<BoatRental>();

            foreach (KeyValuePair<int, Order> entry in this.orders)
            {
                BoatRentalOrder order = entry.Value as BoatRentalOrder;

                if (order != null)
                    boatRentals.Add(order.boatRental);
            }
            return boatRentals;
        }

        public List<GroupTrip> getGroupTrips()
        {
            List<GroupTrip> groupTrips = new List<GroupTrip>();

            foreach (KeyValuePair<int, Order> entry in this.orders)
            {
                GroupTripOrder order = entry.Value as GroupTripOrder;

                if (order != null)
                    groupTrips.Add(order.groupTrip);
            }
            return groupTrips;
        }

        public GroupTripOrder getGroupTripOrder(int orderNumber)
        {
            Order order;

            if (this.orders.TryGetValue(orderNumber, out order) && order is GroupTripOrder)
                return order as GroupTripOrder;

            return null;
        }

        public Order getOrder(int orderNumber)
        {
            Order order;

            if (this.orders.TryGetValue(orderNumber, out order))
                return order;

            return null;
        }

        public List<Order> getAllOrders()
        {
            return this.orders.Values.ToList();
        }

        public List<Location> getLocations()
        {
            return new List<Location>(this.locations);
        }

        public List<Boat> getBoats()
        {
            return new List<Boat>(this.boats);
        }

        public bool addLocation(Location location)
        {
            if (this.locations.Contains(location))
                return false;

            DAL.Location entity = db.Location.Where(v => v.id == location.id).FirstOrDefault();

            if (entity == null && (entity = location.saveInDB()) == null)
                return false;

            //entity.boatToursManagerId = this.id;
            db.SaveChanges();

            this.locations.Add(location);
            return true;
        }

        public bool removeLocation(Location location)
        {
            DAL.Location entity = db.Location.Where(v => v.id == location.id).FirstOrDefault();

            if (entity == null)
                return false;

           // entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.locations.Remove(location);
        }

        public List<Route> getRoutes()
        {
            return new List<Route>(this.routes);
        }

        public bool addRoute(Route route)
        {
            if (route is ScheduledRoute)
                return false;

            if (this.routes.Contains(route))
                return false;

            DAL.Route entity = db.Route.Where(v => v.id == route.id).FirstOrDefault();

            if (entity == null && (entity = route.saveInDB()) == null)
                return false;

           // entity.boatToursManagerId = this.id;
            db.SaveChanges();

            this.routes.Add(route);
            return true;
        }

        public bool removeRoute(Route route)
        {
            DAL.Route entity = db.Route.Where(v => v.id == route.id).FirstOrDefault();

            if (entity == null)
                return false;

            //entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.routes.Remove(route);
        }

        public List<Ship> getShips()
        {
            return new List<Ship>(this.ships);
        }

        public bool addShip(Ship ship)
        {
            if (this.ships.Contains(ship))
                return false;

            DAL.Ship entity = db.Ship.Where(v => v.id == ship.id).FirstOrDefault();

            if (entity == null && (entity = ship.saveInDB()) == null)
                return false;

            //entity.boatToursManagerId = this.id;
            db.SaveChanges();

            this.ships.Add(ship);
            return true;
        }

        public bool removeShip(Ship ship)
        {
            DAL.Ship entity = db.Ship.Where(v => v.id == ship.id).FirstOrDefault();

            if (entity == null)
                return false;

            //entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.ships.Remove(ship);
        }

        public List<PersonCategory> getPersonCategories()
        {
            return new List<PersonCategory>(this.personCategories);
        }

        public bool addPersonCategory(PersonCategory personCategory)
        {
            if (this.personCategories.Contains(personCategory))
                return false;

            DAL.PersonCategory entity = db.PersonCategory.Where(v => v.id == personCategory.id).FirstOrDefault();

            if (entity == null && (entity = personCategory.saveInDB()) == null)
                return false;

           // entity.boatToursManagerId = this.id;
            db.SaveChanges();

            this.personCategories.Add(personCategory);
            return true;
        }

        public bool removePersonCategory(PersonCategory personCategory)
        {
            DAL.PersonCategory entity = db.PersonCategory.Where(v => v.id == personCategory.id).FirstOrDefault();

            if (entity == null)
                return false;

            //entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.personCategories.Remove(personCategory);
        }

        public List<PriceCategory> getPriceCategories()
        {
            return new List<PriceCategory>(this.priceCategories);
        }

        public bool addPriceCategory(PriceCategory priceCategory)
        {
            if (!this.personCategories.Contains(priceCategory.personCategory) ||
                !this.tripTypes.Contains(priceCategory.tripType) ||
                !this.routes.Contains(priceCategory.route))
                return false;

            if (this.priceCategories.Contains(priceCategory))
                return false;

            DAL.PriceCategory entity = db.PriceCategory.Where(v => v.id == priceCategory.id).FirstOrDefault();

            if (entity == null && (entity = priceCategory.saveInDB()) == null)
                return false;

            //entity.boatToursManagerId = this.id;
            db.SaveChanges();

            this.priceCategories.Add(priceCategory);
            return true;
        }

        public bool removePriceCategory(PriceCategory priceCategory)
        {
            DAL.PriceCategory entity = db.PriceCategory.Where(v => v.id == priceCategory.id).FirstOrDefault();

            if (entity == null)
                return false;

            //entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.priceCategories.Remove(priceCategory);
        }

        public List<User> getUsers()
        {
            return new List<User>(this.users);
        }

        public bool addUser(User user)
        {
            if (this.users.Contains(user))
                return false;

            DAL.User entity = db.User.Where(v => v.id == user.id).FirstOrDefault();

            if (entity == null && (entity = user.saveInDB()) == null)
                return false;

            //entity.boatToursManagerId = this.id;
            db.SaveChanges();
            this.users.Add(user);
            return true;
        }

        public bool removeUser(User user)
        {
            DAL.User entity = db.User.Where(v => v.id == user.id).FirstOrDefault();

            if (entity == null)
                return false;

            //entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.users.Remove(user);
        }

        public List<SchedulePlan> getSchedulePlans()
        {
            return new List<SchedulePlan>(this.schedulePlans);
        }

        public bool addSchedulePlan(SchedulePlan schedulePlan)
        {
            if (this.schedulePlans.Contains(schedulePlan))
                return false;

            DAL.SchedulePlan entity = db.SchedulePlan.Where(v => v.id == schedulePlan.id).FirstOrDefault();

            if (entity == null && (entity = schedulePlan.saveInDB()) == null)
                return false;

           // entity.boatToursManagerId = this.id;
            db.SaveChanges();
            this.schedulePlans.Add(schedulePlan);
            return true;
        }

        public bool removeSchedulePlan(SchedulePlan schedulePlan)
        {
            DAL.SchedulePlan entity = db.SchedulePlan.Where(v => v.id == schedulePlan.id).FirstOrDefault();

            if (entity == null)
                return false;

           // entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.schedulePlans.Remove(schedulePlan);
        }

        public List<TripType> getTripTypes()
        {
            return new List<TripType>(this.tripTypes);
        }

        public bool addTripType(TripType tripType)
        {
            if (this.tripTypes.Contains(tripType))
                return false;

            DAL.TripType entity = db.TripType.Where(v => v.id == tripType.id).FirstOrDefault();

            if (entity == null && (entity = tripType.saveInDB()) == null)
                return false;

            //entity.boatToursManagerId = this.id;
            db.SaveChanges();
            this.tripTypes.Add(tripType);
            return true;
        }

        public bool removeTripType(TripType tripType)
        {
            DAL.TripType entity = db.TripType.Where(v => v.id == tripType.id).FirstOrDefault();

            if (entity == null)
                return false;

           // entity.boatToursManagerId = null;
            db.SaveChanges();
            return this.tripTypes.Remove(tripType);
        }
    }
}