using iMMAP.iMPACT.Helpers;
using iMMAP.iMPACT.Identity;
using iMMAP.iMPACT.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iMMAP.iMPACT.Controllers
{
    [Authorize(Roles = "Officer")]
    public class ManageController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Skin = "red";
            return View();
        }

        public ActionResult Products()
        {

            ViewBag.Skin = "red";
            return View();
        }
    }
}