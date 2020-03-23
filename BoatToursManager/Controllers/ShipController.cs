using BoatToursManager.BL;
using BoatToursManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoatToursManager.Controllers
{
    public class ShipController : Controller
    {
        // GET: Ship
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetShip()
        {
            ShipModel ship = new ShipModel
            {
                name = "ship",
                capacity = 200
            };
            return View("GetShip", ship);
        }
        public ActionResult ViewShip()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View(MainClass.Instance.getShips());
        }
        public ActionResult AddShip()
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            return View();
        }
        [HttpPost]
        public ActionResult AddShip(ShipModel shipModel)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (BL.MainClass.Instance.addShip(new BL.Ship(shipModel.capacity, shipModel.name))) {
                ViewBag.Status = true;
                ViewBag.Message = "Ship successfully added";
                return View();
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not add ship";
            return View();
        }
        public ActionResult EditShip(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            BL.Ship s = MainClass.Instance.getShips().Find(v => v.id == id);

            if (s != null) {
                return View(new ShipModel() {
                    name = s.name,
                    capacity = s.capacity??0
                });
            }
            return new HttpNotFoundResult();
        }
        [HttpPost]
        public ActionResult EditShip(ShipModel model)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid) {
                BL.Ship s = MainClass.Instance.getShips().Find(v => v.id == model.id);

                if (s != null) {
                    s.name = model.name;
                    s.capacity = model.capacity;

                    if (s.saveInDB() != null)
                        return RedirectToAction("ViewShip", "Ship");
                }
            }
            ViewBag.Status = false;
            ViewBag.Message = "Could not edit ship";
            return new HttpNotFoundResult();
        }
        public ActionResult DeleteShip(int? id)
        {
            if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                return new HttpStatusCodeResult(403);

            if (!MainClass.Instance.removeShip(MainClass.Instance.getShips().Find(v => v.id == id)))
                return new HttpNotFoundResult();

            return RedirectToAction("ViewShip", "Ship");
        }
    }
}