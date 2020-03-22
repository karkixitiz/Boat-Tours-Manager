using BoatToursManager.BL;
using BoatToursManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoatToursManager.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Login";
            return View();
        }
        public ActionResult UserLogin()
        {
            HttpCookie cookie = Request.Cookies["login"];

            if (cookie != null)
            {
                ViewBag.email = cookie["emailID"].ToString();
                ViewBag.password = cookie["password"].ToString();
                ViewBag.rememberMe = cookie["rememberMe"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(UserLoginModel userLogin)
        {
            if (userLogin.email != null && userLogin.password != null)
            {
                BL.User user = MainClass.Instance.getUsers().Find(v => v.email == userLogin.email);

                if (user != null)

                    if (user.isEmailVerified)
                    {
                        HttpCookie cookie = new HttpCookie("login");
                        cookie.Expires = DateTime.Now.AddDays(-1);

                        HttpCookie oldCookie = Request.Cookies["login"];

                        //if (oldCookie == null || DateTime.Compare(oldCookie.Expires, DateTime.Today) > 0)
                        //{
                        //    if (string.Compare(userLogin.password, user.password) == 0)
                        //    {
                        //        if (userLogin.rememberMe)
                        //        {
                        //            cookie.Values.Add("emailID", user.email);
                        //            cookie.Values.Add("password", user.password);
                        //            cookie.Values.Add("rememberMe", (true).ToString());
                        //            cookie.Expires = DateTime.Now.AddDays(1);
                        //        }
                        //        else
                        //        {
                        //            ViewBag.rememberMe = (false).ToString();
                        //        }
                        //        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                        //        System.Web.HttpContext.Current.Session["user"] = user;
                        //        System.Web.HttpContext.Current.Session["emailID"] = user.name;
                        //        System.Web.HttpContext.Current.Session["userID"] = user.id;
                        //        System.Web.HttpContext.Current.Session["userName"] = user.name;
                        //        System.Web.HttpContext.Current.Session["userType"] = user.userType;
                        //        ViewBag.Message = "User Login Successfully !!!";
                        //        ViewBag.Status = true;
                        //        return RedirectToAction("BookGroupTrip", "Trip");
                        //    }
                        //}
                        //else
                        //{
                        if (string.Compare(CryptoPass.Hash(userLogin.password), user.password) == 0)
                        {
                            if (userLogin.rememberMe)
                            {
                                cookie.Values.Add("emailID", user.email);
                                cookie.Values.Add("password", user.password);
                                cookie.Values.Add("rememberMe", (true).ToString());
                                cookie.Expires = DateTime.Now.AddDays(1);
                            }
                            else
                            {
                                cookie.Expires = DateTime.Now.AddDays(-1);
                                ViewBag.rememberMe = (false).ToString();
                            }
                            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                            System.Web.HttpContext.Current.Session["user"] = user;
                            System.Web.HttpContext.Current.Session["emailID"] = user.name;
                            System.Web.HttpContext.Current.Session["userID"] = user.id;
                            System.Web.HttpContext.Current.Session["userName"] = user.name;
                            System.Web.HttpContext.Current.Session["userType"] = user.userType;

                            ViewBag.Message = "User Login Successfully !!!";
                            ViewBag.Status = true;
                            return RedirectToAction("BookGroupTrip", "Trip");
                        }
                    }

            }
            ViewBag.Message = "Invalid credential provided OR Account is not verified !!!";
            ViewBag.Status = false;
            return View();
        }
    }
}