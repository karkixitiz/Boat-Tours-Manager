using BoatToursManager.BL;
using BoatToursManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoatToursManager.Constants;

namespace BoatToursManager.Controllers
{
    public class TripController : Controller
    {
        // GET: Trip
        public ActionResult ViewTripType()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View(MainClass.Instance.getTripTypes());
        }
        public ActionResult AddTripType()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult ViewRoute()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View(MainClass.Instance.getRoutes());
        }

        public ActionResult EditRoute(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            BL.Route r = MainClass.Instance.getRoutes().Find(v => v.id == id);

            if (r != null) {
                System.Web.HttpContext.Current.Session["startPointLatitude"] = r.startPoint.latitude;
                System.Web.HttpContext.Current.Session["startPointLongitude"] = r.startPoint.longitude;
                r.startPoint.saveInDB();

                System.Web.HttpContext.Current.Session["endPointLatitude"] = r.endPoint.latitude;
                System.Web.HttpContext.Current.Session["endPointLongitude"] = r.endPoint.longitude;
                r.endPoint.saveInDB();

                return View(new RouteModel() {
                    driveTimeMinutes = r.driveTimeMinutes,
                    endPointName = r.endPoint.name,
                    startPointName = r.startPoint.name,
                    name = r.getRouteString(),
                    pointList = new List<RouteLatLongModel>() {
                        new RouteLatLongModel() {
                            latitude = (double) r.startPoint.latitude,
                            longitude = (double) r.startPoint.longitude,
                            title = "Start Point",
                            zIndex = 0
                        },
                        new RouteLatLongModel() {
                            latitude = (double) r.endPoint.latitude,
                            longitude = (double) r.endPoint.longitude,
                            title = "End Point",
                            zIndex = 1
                        }
                    }
                });
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult EditRoute(RouteModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid) {
                BL.Route r = MainClass.Instance.getRoutes().Find(v => v.id == model.id);

                if (r != null) {
                    r.driveTimeMinutes = model.driveTimeMinutes;

                    r.startPoint.name = model.startPointName;
                    r.startPoint.latitude = (decimal) System.Web.HttpContext.Current.Session["startPointLatitude"];
                    r.startPoint.longitude = (decimal) System.Web.HttpContext.Current.Session["startPointLongitude"];
                    r.startPoint.saveInDB();

                    r.endPoint.name = model.endPointName;
                    r.endPoint.latitude = (decimal) System.Web.HttpContext.Current.Session["endPointLatitude"];
                    r.endPoint.longitude = (decimal) System.Web.HttpContext.Current.Session["endPointLongitude"];
                    r.endPoint.saveInDB();

                    if (r.saveInDB() != null)
                        return RedirectToAction("ViewRoute", "Trip");
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not edit route";
            return View();
        }

        public ActionResult DeleteRoute(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (!MainClass.Instance.removeRoute(MainClass.Instance.getRoutes().Find(v => v.id == id)))
                return new HttpNotFoundResult();

            return RedirectToAction("ViewRoute", "Trip");
        }

        [HttpPost]
        public ActionResult AddTripType(TripTypeModel tripTypeModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid) {
                if (BL.MainClass.Instance.addTripType(new BL.TripType(tripTypeModel.name, tripTypeModel.driveTimeMultiplier))) {
                    ViewBag.Status = true;
                    ViewBag.Message = "Trip type successfully added";
                    return View();
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not add trip type";
            return View();
        }

        public ActionResult EditTripType(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            BL.TripType tt = MainClass.Instance.getTripTypes().Find(v => v.id == id);

            if (tt != null) {
                return View(new TripTypeModel() {
                    driveTimeMultiplier = Convert.ToDecimal(tt.driveTimeMultiplier),
                    name = tt.name
                });
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult EditTripType(TripTypeModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid) {
                BL.TripType tt = MainClass.Instance.getTripTypes().Find(v => v.id == model.id);

                if (tt != null) {
                    tt.driveTimeMultiplier = model.driveTimeMultiplier;
                    tt.name = model.name;

                    if (tt.saveInDB() != null)
                        return RedirectToAction("ViewTripType", "Trip");
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not edit trip type";
            return View();
        }

        public ActionResult DeleteTripType(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (!MainClass.Instance.removeTripType(MainClass.Instance.getTripTypes().Find(v => v.id == id)))
                return new HttpNotFoundResult();

            return RedirectToAction("ViewTripType", "Trip");
        }

        private ActionResult initialGroupTripView()
        {
            GroupTripModel gmodel = new GroupTripModel();
            #region Get Ship List
            gmodel.shipList = BL.MainClass.Instance.getShips();
            #endregion
            #region Get Route
            gmodel.routeList = new List<RouteModel>();
            foreach (BL.Route route in BL.MainClass.Instance.getRoutes()) {
                gmodel.routeList.Add(new RouteModel() {
                    id = route.id,
                    name = route.getRouteString()
                });
            }
            #endregion
            #region Get TripType
            gmodel.tripList = BL.MainClass.Instance.getTripTypes();
            #endregion
            #region Get price category
            gmodel.priceCategories = BL.MainClass.Instance.getPriceCategories();
            #endregion
            return View(gmodel);
        }

        [HttpGet]
        public JsonResult GetShipCapacity(int shipId)
        {
            if (!SessionManager.userIsLoggedIn())
                return Json(null, JsonRequestBehavior.AllowGet);

            BL.Ship s = MainClass.Instance.getShips().Find(v => v.id == shipId);

            if (s != null)
                return Json(s.capacity, JsonRequestBehavior.AllowGet);
            
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookGroupTrip()
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            return initialGroupTripView();
        }

        public ActionResult OrderGroupTrip()
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            GroupTripModel groupTrip = (GroupTripModel) System.Web.HttpContext.Current.Session["groupTrip"];

            if (groupTrip != null)
                return View(groupTrip);

            return new HttpStatusCodeResult(403);
        }

        [HttpPost]
        public ActionResult BookGroupTrip(GroupTripModel groupTripModel)
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid) {
                Route route = BL.MainClass.Instance.getRoutes().Find(v => v.id == groupTripModel.routeId);
                TripType triptype = BL.MainClass.Instance.getTripTypes().Find(v => v.id == groupTripModel.tripTypeId);
                Ship ship = BL.MainClass.Instance.getShips().Find(v => v.id == groupTripModel.shipTypeId);

                // Select dummy price categories because of project finish time reasons
                PriceCategory children = MainClass.Instance.getPriceCategories().Find(v => v.name == "Children");
                if (children == null) {
                    BL.PersonCategory pc = new BL.PersonCategory("Children");
                    MainClass.Instance.addPersonCategory(pc);

                    MainClass.Instance.addPriceCategory(new BL.PriceCategory("Children", triptype, pc, 4, route));
                }

                PriceCategory adults = MainClass.Instance.getPriceCategories().Find(v => v.name == "Adults");
                if (adults == null) {
                    BL.PersonCategory pc = new BL.PersonCategory("Adults");
                    MainClass.Instance.addPersonCategory(pc);

                    MainClass.Instance.addPriceCategory(new BL.PriceCategory("Adults", triptype, pc, 7, route));
                }
                BL.GroupTrip groupTrip = new GroupTrip(route, groupTripModel.depatureTime, triptype, ship);

                groupTrip.setPersonsOnBoard(children, groupTripModel.children);
                groupTrip.setPersonsOnBoard(adults, groupTripModel.adults);

                if (MainClass.Instance.canOrderGroupTrip(groupTrip)) {
                    groupTripModel.totalPrice = groupTrip.getTotalPrice();
                    groupTripModel.ship = ship;
                    groupTripModel.tripType = triptype;
                    groupTripModel.route = route;
                    groupTripModel.finalGroupTrip = groupTrip;
                    groupTripModel.routeName = route.getRouteString();
                    groupTripModel.adultPrice = adults.price;
                    groupTripModel.childPrice = children.price;
                    groupTripModel.driveTimeMinutes = (int) groupTrip.getTotalDriveTimeMinutes();
                    groupTripModel.returnTime = groupTrip.returnTime;

                    System.Web.HttpContext.Current.Session["groupTrip"] = groupTripModel;
                    return RedirectToAction("OrderGroupTrip", "Trip");
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Group trip can not be ordered. Please make sure you enter the correct depature time " +
                "and number of persons not bigger than the capacity according to the price category.";
            return initialGroupTripView();
        }
        public ActionResult Payment()
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult ModifyRouteLatLongDummyVals(RouteLatLongStringModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (model.id == 0) { // Start Point
                System.Web.HttpContext.Current.Session["startPointLatitude"] =
                decimal.Parse(model.latitude, System.Globalization.CultureInfo.InvariantCulture);
                System.Web.HttpContext.Current.Session["startPointLongitude"] =
                decimal.Parse(model.longitude, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (model.id == 1) { // End Point 
                System.Web.HttpContext.Current.Session["endPointLatitude"] =
                decimal.Parse(model.latitude, System.Globalization.CultureInfo.InvariantCulture);
                System.Web.HttpContext.Current.Session["endPointLongitude"] =
                decimal.Parse(model.longitude, System.Globalization.CultureInfo.InvariantCulture);
            }
            return new HttpStatusCodeResult(200);
        }

        private ActionResult initialRouteView()
        {
            System.Web.HttpContext.Current.Session["startPointLatitude"] = (decimal) GoogleMaps.INITIAL_LATITUDE;
            System.Web.HttpContext.Current.Session["startPointLongitude"] = (decimal) GoogleMaps.INITIAL_LONGITUDE;

            System.Web.HttpContext.Current.Session["endPointLatitude"] = (decimal) (GoogleMaps.INITIAL_LATITUDE + 0.003);
            System.Web.HttpContext.Current.Session["endPointLongitude"] = (decimal) (GoogleMaps.INITIAL_LONGITUDE + 0.003);

            return View(new RouteModel() {
                pointList = new List<RouteLatLongModel>() {
                    new RouteLatLongModel() {
                        latitude = GoogleMaps.INITIAL_LATITUDE,
                        longitude = GoogleMaps.INITIAL_LONGITUDE,
                        title = "Start Point",
                        zIndex = 0
                    },
                    new RouteLatLongModel() {
                        latitude = GoogleMaps.INITIAL_LATITUDE + 0.003,
                        longitude = GoogleMaps.INITIAL_LONGITUDE + 0.003,
                        title = "End Point",
                        zIndex = 1
                    }
                }
            });
        }
        public ActionResult AddRoute()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return initialRouteView();
        }

        [HttpPost]
        public ActionResult AddRoute(RouteModel routeModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid) {
                decimal longitude = (decimal) System.Web.HttpContext.Current.Session["startPointLongitude"];
                decimal latitude = (decimal) System.Web.HttpContext.Current.Session["startPointLongitude"];

                BL.LatLongCoordinate startPoint = new BL.LatLongCoordinate(
                    (decimal) System.Web.HttpContext.Current.Session["startPointLatitude"],
                    (decimal) System.Web.HttpContext.Current.Session["startPointLongitude"],
                    routeModel.startPointName);
                startPoint.saveInDB();

                BL.LatLongCoordinate endPoint = new BL.LatLongCoordinate(
                    (decimal) System.Web.HttpContext.Current.Session["endPointLatitude"],
                    (decimal) System.Web.HttpContext.Current.Session["endPointLongitude"],
                    routeModel.endPointName);
                endPoint.saveInDB();

                if (MainClass.Instance.addRoute(new BL.Route(startPoint, endPoint, routeModel.driveTimeMinutes))) {
                    ViewBag.Message = "Successfully added route";
                    ViewBag.Status = true;
                    return initialRouteView();
                }
            }
            ViewBag.Message = "Route could not be added";
            ViewBag.Status = false;
            return initialRouteView();
        }
    }
}