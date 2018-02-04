using iMMAP.iMPROVE.Identity;
using iMMAP.iMPROVE.Models.ViewModels;
using iMMAP.iMPROVE.Core.Services;
using iMMAP.iMPROVE.Core.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iMMAP.iMPROVE.Models;
using System.IO;
using System.Web.Script.Serialization;
using iMMAP.iMPROVE.Hubs;
using Ionic.Zip;

namespace iMMAP.iMPROVE.Controllers
{
    [Authorize]
    public class ManageController : ControllerBase
    {
        IUsersService usersService = null;
        IUsersDataService usersDataService = null;
        IIOService ioService = null;
        IProductsService productsService = null;
        IProgramsService programsService = null;
        IMessagesService messagesService = null;

        public ManageController(IUsersService usersService, IUsersDataService usersDataService, IIOService ioService, IProductsService productsService, IProgramsService programsService, IMessagesService messagesService)
        {
            this.ioService = ioService;
            this.usersService = usersService;
            this.usersDataService = usersDataService;
            this.productsService = productsService;
            this.programsService = programsService;
            this.messagesService = messagesService;

            this.messagesService.MessageGenerated += MessagesService_MessageGenerated;
            this.messagesService.NotificationGenerated += MessagesService_NotificationGenerated;
        }

        private void MessagesService_NotificationGenerated(object sender, NotificationEventArgs e)
        {
            MessagesHub.Notify(e.Recipients, e.Body, e.Time);
        }

        private void MessagesService_MessageGenerated(object sender, NotificationEventArgs e)
        {
            MessagesHub.Message(e.Recipients, e.Body, e.Time);
        }

        public ActionResult Index()
        {
            ViewBag.Skin = "red";
            return View();
        }

        #region products actions

        [Authorize(Roles = "User")]
        public ActionResult Products()
        {
            var userOffice = usersDataService.Data.Find<IdentityUser>(s => s.Id == User.Identity.GetUserId(), "Office");
            var model = new ProductsViewModel()
            {
                Products = productsService.GetProducts(User.Identity.GetUserName()),
                ApprovalStatusList = productsService.GetApprovalStatusList(User.Identity.GetUserName()),
                User = new UserViewModel()
                {
                    UserId = User.Identity.GetUserId(),
                    UserName = User.Identity.GetUserName()
                }
            };

            ViewBag.Skin = "red";
            return PartialView(model);
        }

        [HttpPost]
        [Authorize(Roles = "User, Manager")]
        public PartialViewResult ProductVersions(Guid id)
        {
            if (!Request.IsAjaxRequest())
                return null;

            var model = productsService.GetVersions(id);
            return PartialView("_ProductVersions", model);
        }

        [Authorize(Roles = "User")]
        public ActionResult AddProduct(ProductType type)
        {
            IdentityUser createdBy = usersService.UserManager.FindByName(User.Identity.GetUserName());
            EditProductViewModel model = new EditProductViewModel()
            {
                Product = new Product() { Id = Guid.NewGuid() },
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

            ViewBag.ProgramId = new SelectList(programsService.GetProgramsByRegionalOffice(createdBy.OfficeId), "Id", "Name");
            ViewBag.Type = EnumExtensions.GetEnumList(typeof(Models.ProductType), type);
            ViewBag.Month = EnumExtensions.GetEnumList(typeof(Models.Months), (Models.Months)(DateTime.Now.Month - 1));
            ViewBag.ReferenceThematic = EnumExtensions.GetEnumList(typeof(Models.ReferenceThematic));
            ViewBag.Level = EnumExtensions.GetEnumList(typeof(Models.AdminLevel));
            ViewBag.Skin = "red";

            return PartialView("EditProduct", model);
        }

        [Authorize(Roles = "User")]
        public ActionResult EditProduct(Guid id)
        {
            if (id != null)
            {
                Product product = productsService.GetProduct(id);
                if (product != null)
                {
                    IdentityUser createdBy = usersService.UserManager.FindByName(product.CreatedBy);
                    EditProductViewModel model = new EditProductViewModel()
                    {
                        EditMode = true,
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
                    ViewBag.Skin = "red";

                    return PartialView(model);
                }
            }

            return View("~/Views/Shared/_Error404.cshtml");
        }

        [Authorize(Roles = "User")]
        public JsonResult SaveProduct(EditProductViewModel model)
        {
            if(model != null)
            {
                if(!model.EditMode)
                {
                    model.Product.CreatedBy = User.Identity.GetUserName();
                    productsService.AddProduct(model.Product);

                    return Message("Save", "New product has been created");
                }
                else
                {
                    productsService.UpdateProductExcept(model.Product, new string[] {
                        "Id", "CreatedBy", "ActiveVersionProductId"
                    });
                    return Message("Update", "The product has been updated");
                }
            }
            return Error();
        }

        [Authorize(Roles = "User")]
        public JsonResult AddProductVersion(ProductVersionViewModel model)
        {
            if (!Request.IsAjaxRequest())
                return Error("This action is for ajax calls");

            if (model != null && model.ProductId != Guid.Empty)
            {
                if (Session["uploadsession.addversion." + model.ProductId] != null)
                {
                    string filePath = "~/" + Session["uploadsession.addversion." + model.ProductId].ToString();
                    string disPath = Server.MapPath(filePath);

                    using (ZipFile zip = new ZipFile(disPath))
                    {
                        string extractPath = Directory.GetParent(disPath).FullName;
                        zip.ExtractAll(extractPath, ExtractExistingFileAction.DoNotOverwrite);
                    }

                    if (System.IO.File.Exists(disPath))
                    {
                        Session.Remove("uploadsession.addversion." + model.ProductId);
                        Guid id = productsService.AddVersion(new ProductVersion()
                        {
                            Description = model.Description,
                            Changes = model.Changes,
                            Date = DateTime.Now,
                            ProductId = model.ProductId,
                        });

                        return Message("New Product", "New product version has been added");
                    }
                }
                else
                {
                    return Error("You didn't select a file to upload");
                }
            }

            return Error();
        }

        [Authorize(Roles = "User")]
        public JsonResult DeleteRequest(Guid id, string message)
        {
            if (!Request.IsAjaxRequest())
                return Error("This action is for ajax calls");

            if (id != Guid.Empty)
            {
                productsService.SendDeleteProductRequest(id, message);
                return Message("Delete Product Request", "Your request to delete this product has been sent.");
            }

            return Error();
        }

        #endregion

        #region notifications

        [Authorize(Roles = "User, Manager, Administrator")]
        public ActionResult Notifications()
        {
            ViewBag.Skin = "red";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User, Manager, Administrator")]
        public PartialViewResult NotificationsTable()
        {
            if (!Request.IsAjaxRequest())
                return null;

            var model = messagesService.GetNotifications(User.Identity.GetUserName());
            return PartialView("_NotificationsTable", model);
        }

        [Authorize(Roles = "User, Manager, Administrator")]
        public JsonResult AknowledgeNotification(Guid id)
        {
            messagesService.AknowledgeNotification(id);
            return Message("Notification Acknowledgement", "You have acknowledged the notification");
        }

        #endregion

        #region IO

        [Authorize(Roles = "User, Manager, Administrator")]
        public FileResult Resource(string path)
        {
            return null;
        }

        [Authorize(Roles = "User, Manager, Administrator")]
        public ContentResult UploadFiles(List<HttpPostedFileBase> files, string dir, bool overwrite = false)
        {
            JsonResult result = new JsonResult()
            {
                Data = new
                {
                    title = "Files Upload",
                    message = "Files upload operation has encountered an error!",
                    failure = true
                }
            };

            try
            {
                string filesPath = System.IO.Path.Combine(Server.MapPath("~/Files"), dir);

                foreach (var file in files)
                {
                    if (file.ContentLength > 0)
                    {
                        string filePath = Path.Combine(filesPath, file.FileName);
                        bool toOverwrite = overwrite && System.IO.File.Exists(filePath);
                        if (toOverwrite)
                            filePath = ioService.GetUniqueFilePath(filePath);

                        string fileDir = Path.GetDirectoryName(filePath);

                        if (!Directory.Exists(fileDir))
                            Directory.CreateDirectory(fileDir);

                        if (toOverwrite)
                            System.IO.File.Delete(filePath);

                        file.SaveAs(filePath);

                        result = new JsonResult()
                        {
                            Data = new
                            {
                                title = "File Upload",
                                message = "Files has been uploaded successfully!"
                            }
                        };
                    }
                }
            }
            catch
            {
            }

            ContentResult contentResult = new ContentResult()
            {
                Content = new JavaScriptSerializer().Serialize(result.Data),
            };
            return contentResult;
        }

        [Authorize(Roles = "User, Manager, Administrator")]
        public JsonResult UploadFile(HttpPostedFileBase file, string code, string prefix, string dir, string tag, bool overwrite = false, bool resource = false)
        {
            string fileName = string.Empty;
            try
            {
                if (null != file && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    string sessionPrefix = "uploadsession." + (!string.IsNullOrEmpty(tag) ? tag + "." : "");
                    string filesDir = Server.MapPath("~/Files");
                    string targetDir = System.IO.Path.Combine(filesDir, dir);
                    if (Directory.Exists(targetDir))
                    {
                        long maxSize = 26214400;
                        if ((file.ContentLength) > maxSize)
                        {
                            if (!string.IsNullOrEmpty(code))
                                Session.Add(sessionPrefix + code, "error:max");

                            return Json(new
                            {
                                file = fileName,
                                title = "File Upload",
                                message = string.Format("The file exceeds the maximum file size"),
                                failure = true
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(prefix) && prefix != "undefined")
                        fileName = prefix + file.FileName;

                    string filePath = Path.Combine(targetDir, fileName);
                    bool toOverwrite = overwrite && System.IO.File.Exists(filePath);
                    if (!toOverwrite)
                        filePath = ioService.GetUniqueFilePath(filePath);

                    string fileDir = Path.GetDirectoryName(filePath);

                    if (!Directory.Exists(fileDir))
                        Directory.CreateDirectory(fileDir);

                    string path = null;
                    string relativePath = Server.RelativePath(filePath);
                    if (resource)
                    {
                        string[] relativePathParts = relativePath.Trim('/').Split('/');
                        if (relativePathParts.Length > 2)
                            path = string.Format("resource?path={0}",
                                string.Join("/", relativePathParts.Skip(2)));
                    }

                    if (string.IsNullOrEmpty(path))
                        path = relativePath;

                    if (!string.IsNullOrEmpty(code))
                        Session.Add(sessionPrefix + code, path);

                    if (toOverwrite)
                        System.IO.File.Delete(filePath);

                    file.SaveAs(filePath);

                    return Json(new
                    {
                        file = fileName,
                        title = "File Upload",
                        message = string.Format("The file '{0}' has been uploaded successfully, under the name '{1}'!", file.FileName, Path.GetFileName(filePath)),
                        url = filePath
                    });
                }
            }
            catch
            {
                if (null != file)
                {
                    return Json(new
                    {
                        file = fileName,
                        title = "File Upload",
                        message = string.Format("Uploading file '{0}' has failed!", file.FileName),
                        failure = true
                    });
                }
            }

            //ContentResult cr = new ContentResult()
            //{
            //    Content = new JavaScriptSerializer().Serialize(result.Data),
            //    //ContentType = "application/json"
            //};
            return Json(new
            {
                file = fileName,
                title = "File Upload",
                message = string.Format("Uploading file has failed!"),
                failure = true
            });
        }
        #endregion
    }
}