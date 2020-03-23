using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoatToursManager.BL;
using BoatToursManager.Models;
using PayPal.Api;

namespace BoatToursManager.Controllers
{
    public class PayPalController : Controller
    {
        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreateBoatRentalPayment(APIContext apiContext, string redirectUrl, BoatRentModel order)
        {
            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls() {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var shipping = new ShippingAddress() {
                recipient_name = "Schwentine Boat Tours",
                line1 = "An der Holsatiamuehle",
                city = "Kiel",
                country_code = "DE",
                postal_code = "24149",
                phone = "0431722428"
            };

            var itemList = new ItemList() {
                shipping_address = shipping,
                items = new List<Item>() {
                    new Item() {
                        name = "Boat Rental",
                        sku = "br",
                        description = "Your Boat Rental at BoatToursManager with boat \"" + order.boat.name + "\" from location \"" + order.locationName + "\"",
                        price = order.totalPrice.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        quantity = "1",
                        currency = "EUR"
                    }
                }
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount() {
                currency = "EUR",
                total = order.totalPrice.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction() {
                description = "BoatRental order from location \"" + order.locationName + "\" with boat \"" +
                        order.boat.name + "\"",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment() {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls,
                note_to_payer = "Hello, Please pay for your order at the BoatToursManager"
            };
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        private Payment CreateGroupTripPayment(APIContext apiContext, string redirectUrl, GroupTripModel order)
        {
            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls() {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var shipping = new ShippingAddress() {
                recipient_name = "Schwentine Boat Tours",
                line1 = "An der Holsatiamuehle",
                city = "Kiel",
                country_code = "DE",
                postal_code = "24149",
                phone = "0431722428"
            };

            var itemList = new ItemList() {
                shipping_address = shipping,
                items = new List<Item>() {
                    new Item() {
                        name = "Group Trip Order",
                        sku = "gto",
                        description = "Your Group Trip Order at BoatToursManager with ship \"" + order.ship.name + "\"",
                        price = order.totalPrice.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        quantity = "1",
                        currency = "EUR"
                    }
                }
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount() {
                currency = "EUR",
                total = order.totalPrice.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction() {
                description = "GroupTrip order with ship \"" + order.ship.name + "\"",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment() {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentSuccessful()
        {
            ViewBag.Message = "Payment was successful";
            ViewBag.Status = true;
            return View("Payment");
        }

        public ActionResult PaymentWithPaypal()
        {
            if (!SessionManager.userIsLoggedIn())
                return new HttpStatusCodeResult(403);

            BL.User user = (BL.User) System.Web.HttpContext.Current.Session["user"];

            if (user == null) {
                ViewBag.Status = false;
                ViewBag.Message = "User is not logged in";
                return View("Payment");
            }
            BoatRentModel boatRentModel = (BoatRentModel) System.Web.HttpContext.Current.Session["boatRental"];
            GroupTripModel groupTripModel = (GroupTripModel) System.Web.HttpContext.Current.Session["groupTrip"];

            if (boatRentModel == null && groupTripModel == null) {
                ViewBag.Status = false;
                ViewBag.Message = "Neither the BoatRentModel nor the GroupTripModel exists";
                return View("Payment");
            }
            //getting the apiContext as earlier
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId)) {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/PayPal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    Payment createdPayment = null;

                    if (boatRentModel != null)
                        createdPayment = this.CreateBoatRentalPayment(apiContext, baseURI + "guid=" + guid, boatRentModel);
                    else if (groupTripModel != null)
                        createdPayment = this.CreateGroupTripPayment(apiContext, baseURI + "guid=" + guid, groupTripModel);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext()) {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url")) {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved") {
                        ViewBag.Status = false;
                        ViewBag.Message = "Payment with PayPal is not approved.";
                        return View("Payment");
                    }

                    if (boatRentModel != null) {
                        BL.Location location = MainClass.Instance.getLocations().Find(v => v.id == boatRentModel.locationId);

                        if (location == null) {
                            ViewBag.Status = false;
                            ViewBag.Message = "Location could not be found";
                            return View("Payment");
                        }
                        BL.BoatRental br = location.rentBoat(boatRentModel.boat, boatRentModel.startTime, boatRentModel.endTime, boatRentModel.numPersons);

                        if (br == null) {
                            ViewBag.Status = false;
                            ViewBag.Message = "Boat could not be rented";
                            return View("Payment");
                        }
                        if (MainClass.Instance.orderBoatRental(br, PaymentType.PAYPAL, user.userAddress, user) == null) {
                            ViewBag.Status = false;
                            ViewBag.Message = "Boat could not be rented";
                            return View("Payment");
                        }
                        System.Web.HttpContext.Current.Session.Remove("boatRental");
                    }
                    else if (groupTripModel != null) {
                        if (MainClass.Instance.orderGroupTrip(groupTripModel.finalGroupTrip, PaymentType.PAYPAL, user.userAddress, user) == null) {
                            ViewBag.Status = false;
                            ViewBag.Message = "Group trip could not be ordered";
                            return View("Payment");
                        }
                        System.Web.HttpContext.Current.Session.Remove("groupTrip");
                    }
                }
            }
            catch (Exception e) {
                ViewBag.Status = false;
                ViewBag.Message = e.Message;
                return View("Payment");
            }
            ViewBag.Status = true;
            ViewBag.Message = "Payment with PayPal was successful.";
            return View("Payment");
        }
    }
}