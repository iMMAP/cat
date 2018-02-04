using iMMAP.iMPROVE.Core.Extensions;
using iMMAP.iMPROVE.Core.Services;
using iMMAP.iMPROVE.Identity;
using iMMAP.iMPROVE.Models;
using iMMAP.iMPROVE.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iMMAP.iMPROVE.Controllers
{
    [Authorize(Roles = "Manager, Administrator")]
    public class AdminController : ControllerBase
    {
        IUsersDataService usersDataService = null;
        IProductsService productsService = null;
        IProgramsService programsService = null;
        IUsersService usersService = null;

        public AdminController(IProductsService productsService, IUsersDataService usersDataService, IProgramsService programsService, IUsersService usersService)
        {
            this.usersDataService = usersDataService;
            this.productsService = productsService;
            this.programsService = programsService;
            this.usersService = usersService;
        }

        public ActionResult Index()
        {
            ViewBag.Skin = "blue";
            ViewBag.AdminDashboard = true;
            return View();
        }

        public ActionResult Products()
        {
            var model = productsService.GetProductsByManager(User.Identity.GetUserName());
            ViewBag.Skin = "blue";
            return PartialView(model);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Review(Guid id)
        {
            if (id != null)
            {
                Product product = productsService.GetProduct(id);
                if (product != null)
                {
                    IdentityUser createdBy = usersService.UserManager.FindByName(product.CreatedBy);
                    ReviewProductViewModel model = new ReviewProductViewModel()
                    {
                        Product = product,
                        Versions = productsService.GetVersions(id),
                        Office = programsService.GetRegionalOffice(createdBy.OfficeId),
                        CreatedBy = new UserViewModel()
                        {
                            UserName = createdBy.UserName,
                            FullName = createdBy.FullName,
                            Email = createdBy.Email,
                            Phone = createdBy.Phone,
                            UserId = createdBy.Id,
                            Type = createdBy.Type
                        }
                    };

                    ViewBag.ProgramId = new SelectList(programsService.GetProgramsByRegionalOffice(createdBy.OfficeId), "Id", "Name", product.ProgramId);
                    ViewBag.Type = EnumExtensions.GetEnumList(typeof(Models.ProductType), product.Type);
                    ViewBag.Month = EnumExtensions.GetEnumList(typeof(Models.Months), product.Month);
                    ViewBag.ReferenceThematic = EnumExtensions.GetEnumList(typeof(Models.ReferenceThematic), product.ReferenceThematic);
                    ViewBag.Level = EnumExtensions.GetEnumList(typeof(Models.AdminLevel), product.Level);
                    ViewBag.Skin = "blue";

                    return PartialView(model);
                }
            }

            return View("~/Views/Shared/_Error404.cshtml");
        }

        public JsonResult ChangeApproval(Guid versionId, ProductApprovalStatus status, string comments)
        {
            if (!Request.IsAjaxRequest())
                return Error("This action is for ajax calls");

            if (versionId != Guid.Empty)
            {
                productsService.ChangeProductApproval(versionId, new ProductApprovalModel()
                {
                    Status = status,
                    Date = DateTime.Now,
                    Manager = User.Identity.GetUserName(),
                    Comments = comments
                });
                return Message("Delete Product Request", "Your request to delete this product has been sent.");
            }

            return Error();
        }

        public FileResult DownloadPDF(Guid versionId)
        {
            return null;
        }

        public JsonResult VersionConversation(Guid versionId)
        {
            return null;
        }
    }
}