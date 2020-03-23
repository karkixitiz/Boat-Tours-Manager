using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using BoatToursManager.BL;
using BoatToursManager.Models;
using BoatToursManager.Constants;

namespace BoatToursManager.Controllers
{
    public class BoatController : BaseController
    {
        LikeDislike likeDislike = new LikeDislike();
        // GET: Boat
        public ActionResult ViewScheduleRoute()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult AddScheduleRoute()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        [HttpPost]
        public ActionResult AddScheduleRoute(ScheduledRoute routeModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult EditScheduleRoute(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        [HttpPost]
        public ActionResult EditScheduleRoute(ScheduledRoute setRoute)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult DeleteScheduleRoute(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult AddSchedulePlan()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        [HttpPost]
        public ActionResult AddSchedulePlan(SchedulePlanModel schedulePlanModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            // BL.MainClass.Instance.addSchedulePlan(new BL.SchedulePlan(schedulePlanModel.beginDate, schedulePlanModel.endDate));
            return View();
        }



        public ActionResult EditSchedulePlan(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);
            return View();
        }

        [HttpPost]
        public ActionResult EditSchedulePlan(SchedulePlan setPlan)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult DeleteSchedulePlan(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }

        public ActionResult ViewBoat(int? locationId)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (locationId != null)
            {
                BL.Location l = MainClass.Instance.getLocations().Find(v => v.id == locationId);

                if (l != null)
                {
                    ViewBag.locationId = locationId;
                    return View(l.getBoats());
                }
            }
            return new HttpNotFoundResult();
        }

        public ActionResult ViewLocation()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View(MainClass.Instance.getLocations());
        }

        public ActionResult AddBoat(int? locationId)
        {
            var list = DropDownListUtility.GetSeasonDropDown("2"); //select second option by default;
            ViewData["seasonList"] = list;
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (locationId != null && MainClass.Instance.getLocations().Find(v => v.id == locationId) != null)
            {
                ViewBag.locationId = (int)locationId;
                return View(new BoatModel()
                {
                    locationId = (int)locationId,
                    locationList = BL.MainClass.Instance.getLocations()
                });
            }
            return View(new BoatModel() { locationList = BL.MainClass.Instance.getLocations() });
        }

        [HttpPost]
        public ActionResult AddBoat(BoatModel boatModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            boatModel.locationList = BL.MainClass.Instance.getLocations();
            boatModel.file.SaveAs(Server.MapPath("~/Public/Images/") + boatModel.file.FileName);
            boatModel.imagePath= boatModel.file.FileName;

            // string path = Path.Combine(Server.MapPath("~/Public/Images"), Path.GetFileName(boatModel.file.FileName));
            //boatModel.ImageFile.SaveAs(path);

            string fileName = boatModel.file.FileName;
            //string extension = Path.GetExtension(boatModel.file.FileName);
            //fileName = fileName + extension;
            string imagePath = "Public/Images/" + fileName;
            if (ModelState.IsValid)
            {
                BL.Location location = MainClass.Instance.getLocations().Find(v => v.id == boatModel.locationId);

                if (location != null)
                {
                    if (location.addBoat(new BL.Boat(boatModel.name, boatModel.capacity, boatModel.pricePerHour,boatModel.seasonId, imagePath)))
                    {
                        ViewBag.Status = true;
                        ViewBag.Message = "Boat added successfully";

                        return View(boatModel);
                    }
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Boat could not be added. Please make sure that you select a location.";
            return View(boatModel);
        }

        public ActionResult EditBoat(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            List<Location> locations = MainClass.Instance.getLocations();

            foreach (Location l in locations)
            {
                BL.Boat b = l.getBoats().Find(v => v.id == id);

                if (b != null)
                {
                    return View(new BoatModel()
                    {
                        locationId = l.id,
                        capacity = b.capacity,
                        name = b.name,
                        locationList = locations,
                        pricePerHour = b.pricePerHour
                    });
                }
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult EditBoat(BoatModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                Location newLocation = MainClass.Instance.getLocations().Find(v => v.id == model.locationId);

                if (newLocation != null)
                {
                    foreach (Location l in MainClass.Instance.getLocations())
                    {
                        BL.Boat b = l.getBoats().Find(v => v.id == model.id);

                        if (b != null)
                        {
                            b.capacity = model.capacity;
                            b.name = model.name;
                            b.pricePerHour = model.pricePerHour;

                            if (newLocation != l)
                            {
                                if (l.removeBoat(b) && newLocation.addBoat(b) && b.saveInDB() != null)
                                    return Redirect("/Boat/ViewBoat?locationId=" + l.id);
                                else
                                    return View(model);
                            }
                            if (b.saveInDB() != null)
                                return Redirect("/Boat/ViewBoat?locationId=" + l.id);
                        }
                    }
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not edit boat";
            return View(model);
        }

        public ActionResult DeleteBoat(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            foreach (Location l in MainClass.Instance.getLocations())
            {
                BL.Boat b = l.getBoats().Find(v => v.id == id);

                if (b != null && l.removeBoat(b))
                    return Redirect("/Boat/ViewBoat?locationId=" + l.id);
            }
            return new HttpNotFoundResult();
        }
        [HttpGet]
        public ActionResult RentBoat()
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            return View(new BoatRentModel() { locationList = BL.MainClass.Instance.getLocations() });
        }
        public ActionResult GetBoatList(int locationId)
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            BL.Location location = BL.MainClass.Instance.getLocations().Find(v => v.id == locationId);
            ViewBag.boatList = new SelectList(location.getBoats(), "id", "name");
            return PartialView("DisplayBoatInLocation");
        }
        [HttpGet]
        public JsonResult GetBoatDetail(int boatId)
        {
            if (!SessionManager.userIsLoggedIn())
                return Json(null, JsonRequestBehavior.AllowGet);

            foreach (Location l in MainClass.Instance.getLocations())
            {
                BL.Boat b = l.getBoats().Find(v => v.id == boatId);

                if (b != null)
                    return Json(b, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderBoatRental()
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            BoatRentModel rental = (BoatRentModel)System.Web.HttpContext.Current.Session["boatRental"];

            if (rental != null)
                return View(rental);

            return new HttpStatusCodeResult(403);
        }

        [HttpPost]
        public ActionResult RentBoat(BoatRentModel boatRentModel)
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                BL.Location location = MainClass.Instance.getLocations().Find(v => v.id == boatRentModel.locationId);

                if (location != null)
                {
                    BL.Boat boat = location.getBoats().Find(v => v.id == boatRentModel.boatId);

                    if (boat != null)
                    {
                        if (location.canRentBoat(boat, boatRentModel.startTime, boatRentModel.endTime, boatRentModel.numPersons))
                        {
                            BL.BoatRental rental = new BL.BoatRental(boatRentModel.startTime,
                                boatRentModel.endTime, boat, boatRentModel.numPersons);

                            boatRentModel.totalPrice = rental.getTotalPrice();
                            boatRentModel.locationName = location.name;
                            boatRentModel.boat = boat;
                            System.Web.HttpContext.Current.Session["boatRental"] = boatRentModel;
                            return RedirectToAction("OrderBoatRental", "Boat");
                        }
                    }
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Boat can not be rented. Please make sure you enter the correct date and time slot " +
                "and number of persons not bigger than the capacity.";
            return View(new BoatRentModel() { locationList = BL.MainClass.Instance.getLocations() });
        }

        private ActionResult initialLocationView()
        {
            return View(new LocationModel()
            {
                point = new List<LatLongModel> {
                    new LatLongModel { latitude = GoogleMaps.INITIAL_LATITUDE,
                        longitude = GoogleMaps.INITIAL_LONGITUDE } }
            });
        }

        public ActionResult AddLocation()
        {
            System.Web.HttpContext.Current.Session["mapLatitude"] = (decimal)GoogleMaps.INITIAL_LATITUDE;
            System.Web.HttpContext.Current.Session["mapLongitude"] = (decimal)GoogleMaps.INITIAL_LONGITUDE;
            return initialLocationView();
        }

        public ActionResult EditLocation(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            BL.Location l = MainClass.Instance.getLocations().Find(v => v.id == id);

            if (l != null)
            {
                return View(new LocationModel()
                {
                    name = l.name,
                    point = new List<LatLongModel>() {
                        new LatLongModel() {
                            latitude = (double) l.point.latitude,
                            longitude = (double) l.point.longitude
                        }
                    }
                });
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult EditLocation(LocationModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                BL.Location l = MainClass.Instance.getLocations().Find(v => v.id == model.id);

                if (l != null)
                {
                    l.name = model.name;
                    l.point.latitude = (decimal)System.Web.HttpContext.Current.Session["mapLatitude"];
                    l.point.longitude = (decimal)System.Web.HttpContext.Current.Session["mapLongitude"];
                    l.point.saveInDB();

                    if (l.saveInDB() != null)
                        return RedirectToAction("ViewLocation", "Boat");
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not edit location";
            return View();
        }

        public ActionResult DeleteLocation(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (!MainClass.Instance.removeLocation(MainClass.Instance.getLocations().Find(v => v.id == id)))
                return new HttpNotFoundResult();

            return RedirectToAction("ViewLocation", "Boat");
        }

        [HttpPost]
        public ActionResult ModifyLatLongDummyVals(LatLongStringModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            System.Web.HttpContext.Current.Session["mapLatitude"] =
                decimal.Parse(model.latitude, System.Globalization.CultureInfo.InvariantCulture);
            System.Web.HttpContext.Current.Session["mapLongitude"] =
                decimal.Parse(model.longitude, System.Globalization.CultureInfo.InvariantCulture);
            return new HttpStatusCodeResult(200);
        }
        [HttpPost]
        public ActionResult AddLocation(LocationModel locationModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            decimal latitude = (decimal)System.Web.HttpContext.Current.Session["mapLatitude"];
            decimal longitude = (decimal)System.Web.HttpContext.Current.Session["mapLongitude"];

            if (ModelState.IsValid)
            {
                BL.LatLongCoordinate point = new BL.LatLongCoordinate(latitude,
                    longitude, locationModel.name);
                point.saveInDB();

                if (MainClass.Instance.addLocation(new Location(point, locationModel.name)))
                {
                    ViewBag.Message = "Location added successfully";
                    ViewBag.Status = true;
                    return initialLocationView();
                }
            }
            ViewBag.Message = "Location could not be added";
            ViewBag.Status = false;
            return initialLocationView();
        }
        [HttpGet]
        public ActionResult BoatPopularity()
        {
             var id=Request.RequestContext.HttpContext.Session["userId"];
            ViewBag.Message = id;
            LikeDislike ld = new LikeDislike();
            BoatModel m = new BoatModel();
            m.popularityList = ld.getPopularity();
            m.locationList = BL.MainClass.Instance.getLocations();
            return View(m);
        }
        List<CommentModel> a;
        public List<CommentModel> GetBoatDetailsById(int boatId, int commentId)
        {
           
            CommentBL comment = new CommentBL();
            if (boatId != 0)
            {
                 a= comment.GetCommentsByBoatId(boatId);
            }
            return a;
        }
        public ActionResult Like(int id, bool status)
        {
            var Db = new LikeDislike();  //created datafunction.cs   and there implemented  like function
            //favar result = Db.Like(id, status);  // calling and sending data to  like function using Db
            return View();
        }

        public ActionResult details(int boatId) //id is threadid this id we are getting from other page index 
        {
            var l = new LikeDislike();
            ViewBag.like = l.Getlikecounts(boatId);
            ViewBag.Dislike = l.Getdislikecounts(boatId);
            ViewBag.AllUserlikedislike = l.GetallUser(boatId);
            return View();
        }
        public JsonResult GetBoatListByLocationIdAndPopularity(int boatId, string season)
        {
            foreach (Location l in MainClass.Instance.getLocations())
            {
                BL.Boat b = l.getBoats().Find(v => v.id == boatId);

                if (b != null)
                    return Json(b, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadBoatList()
        {
            List<Location> l = MainClass.Instance.getLocations().ToList();
            ViewBag.locationList = new SelectList(l, "id", "name");
            return PartialView("DisplayLocation");
        }
        public bool CheckLocation(int locationId)
        {
            BL.Location l = MainClass.Instance.getLocations().Find(v => v.id == locationId);
            if (l != null)
            {
                return true;
            }
            else
                return false;
        }
        public ActionResult GetBoats()
        {
            return View("Boats");
        }
        public string checkNull(String comment)
        {
            var result = "";
            if (comment == null)
            {
                result = "Empty";
            }
            else
            {
                result = "Not Empty";
            }
            return result;
        }
    }
}