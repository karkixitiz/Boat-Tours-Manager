using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BoatToursManager.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public JsonResult SendResponse(string title, string message, bool isok)
        {
            return Json(new
            {
                Title = title,
                Message = message,
                IsOK = isok
            });
        }
        // return a response simply indicating that the operation executed without errors
        public JsonResult SendResponse(bool isok)
        {
            return Json(new { IsOK = isok });
        }
        // return a response that has an object in it that is required for the next set of operations
        public JsonResult SendResponse(object var, bool isok)
        {
            return Json(new
            {
                Packet = var,
                IsOK = isok
            });
        }

        public JsonResult ReportError(Exception ex, string action = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Error actioning request: [ {0} ]", ex.Message));

            var Result = Json(new
            {
                IsOk = false,
                Title = action,
                Message = ex.Message
            });

            return CreateResponseMessage(Result, true);
        }

        public JsonResult CreateResponseMessage(object data, bool allowGet)
        {
            if (allowGet)
                return Json(data, JsonRequestBehavior.AllowGet);
            else
                return Json(data, JsonRequestBehavior.DenyGet);
        }

        // Always allow GET
        public JsonResult CreateResponseMessage(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}