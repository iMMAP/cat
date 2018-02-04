using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iMMAP.iMPROVE.Controllers
{
    public class ControllerBase : Controller
    {
        // GET: ControllerBase
        public JsonResult Error(string message = "An error occured during the execution of this operation")
        {
            return Json(new
            {
                error = true,
                message = message
            });
        }

        public JsonResult Message(string title, string message, dynamic data = null)
        {
            return Json(new
            {
                title = title,
                message = message,
                data = data
            });
        }
    }
}