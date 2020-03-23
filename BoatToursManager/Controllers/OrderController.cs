using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoatToursManager.BL;
using BoatToursManager.Models;

namespace BoatToursManager.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewOrder(bool? byUser)
        {
            if (byUser == true) {
                BL.User u = (BL.User) System.Web.HttpContext.Current.Session["user"];

                if (u != null) {
                    List<OrderModel> orders = new List<OrderModel>();

                    foreach (Order o in MainClass.Instance.getAllOrders().FindAll(v => v.user == u)) {
                        OrderModel m = new OrderModel() {
                            orderDate = o.orderDate,
                            orderNumber = o.orderNumber,
                            orderType = o is BoatRentalOrder ? "Boat Rental Order" : "Group Trip Order",
                            paymentType = o.paymentType.ToString(),
                            price = o.price,
                            userName = o.user.name
                        };
                        orders.Add(m);
                    }
                    return View(orders);
                }
            }
            else {
                if (!SessionManager.checkCurrentUserType(UserType.MAINTENANCE_PERSON))
                    return new HttpStatusCodeResult(403);

                List<OrderModel> orders = new List<OrderModel>();

                foreach (Order o in MainClass.Instance.getAllOrders()) {
                    OrderModel m = new OrderModel() {
                        orderDate = o.orderDate,
                        orderNumber = o.orderNumber,
                        orderType = o is BoatRentalOrder ? "Boat Rental Order" : "Group Trip Order",
                        paymentType = o.paymentType.ToString(),
                        price = o.price,
                        userName = o.user.name
                    };
                    orders.Add(m);
                }
                return View(orders);
            }
            return new HttpStatusCodeResult(403);
        }
    }
}