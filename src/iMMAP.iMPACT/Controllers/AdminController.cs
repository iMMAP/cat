using iMMAP.iMPACT.Helpers;
using iMMAP.iMPACT.Identity;
using iMMAP.iMPACT.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iMMAP.iMPACT.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Skin = "blue";
            ViewBag.AdminDashboard = true;
            return View();
        }
    }
}