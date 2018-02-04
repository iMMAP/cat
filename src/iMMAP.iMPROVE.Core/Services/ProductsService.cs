using iMMAP.iMPROVE.Identity;
using iMMAP.iMPROVE.Models;
using iMMAP.iMPROVE.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;

namespace iMMAP.iMPROVE.Core.Services
{
    public interface IProductsService
    {
        List<Product> GetProducts();
        List<Product> GetProducts(string userId);
        List<Product> GetProductsByManager(string managerId);
        Product GetProduct(Guid id);
        Guid AddProduct(Product product);

        List<ProductApprovalViewModel> GetApprovalStatusList(string userId);
        List<ProductApprovalViewModel> GetApprovalStatusListByManager(string managerId);

        Guid AddVersion(ProductVersion version);
        List<ProductVersion> GetVersions(Guid id);

        void UpdateProduct(Product product, string[] fields = null);
        void UpdateProductExcept(Product product, string[] fields);
        void SendDeleteProductRequest(Guid id, string message);
        bool PositiveDeleteProduct(Guid id, string confirmedBy);
        void ChangeProductApproval(Guid versionId, ProductApprovalModel status);
    }

    public class ProductsService: IProductsService
    {
        private IDataService db = null;
        private IUsersDataService usersDb = null;
        private IMessagesService messagesService = null;

        public ProductsService(IDataService db, IUsersDataService usersDb, IMessagesService messagesService)
        {
            this.db = db;
            this.usersDb = usersDb;
            this.messagesService = messagesService;
        }

        public Guid AddProduct(Product product)
        {
            if (product == null)
                return Guid.Empty;

            Dictionary<string, object> keys = (Dictionary<string, object>)db.Data.Insert(product);
            if(keys.ContainsKey("Id"))
            {
                //find and notify user direct managers
                var managedUserGroup = usersDb.Data.Find<ManagedUsersGroup>(s => s.Users.Contains(product.CreatedBy));
                if (managedUserGroup != null && managedUserGroup.Managers != null && managedUserGroup.Managers.Any())
                {
                    messagesService.Notify(
                        managedUserGroup.Managers.Concat(new string[] { product.CreatedBy }).ToArray(),
                        string.Format("@{0} created a new {1} product ({2})", product.CreatedBy, product.Type, product.Name),
                        DateTime.Now);
                }
                return (Guid)keys["Id"];
            }
            return Guid.Empty;
        }

        public Guid AddVersion(ProductVersion version)
        {
            if (version == null)
                return Guid.Empty;

            var product = GetProduct(version.ProductId);
            if (product == null)
                return Guid.Empty;

            Dictionary<string, object> keys = (Dictionary<string, object>)db.Data.Insert(version);
            if (keys.ContainsKey("Id"))
            {
                //find and notify user direct managers
                var managedUserGroup = usersDb.Data.Find<ManagedUsersGroup>(s => s.Users.Contains(product.CreatedBy));
                if (managedUserGroup != null && managedUserGroup.Managers != null && managedUserGroup.Managers.Any())
                {
                    messagesService.Notify(
                        managedUserGroup.Managers.Concat(new string[] { product.CreatedBy }).ToArray(),
                        string.Format("@{0} added a new {1} product version for ({2})", product.CreatedBy, product.Type, product.Name),
                        DateTime.Now);
                }
                return (Guid)keys["Id"];
            }
            return Guid.Empty;
        }

        public List<ProductVersion> GetVersions(Guid id)
        {
            return db.Data.Query<ProductVersion>(s => s.ProductId == id).ToList();
        }

        public void ChangeProductApproval(Guid versionId, ProductApprovalModel approvalModel)
        {
            var version = db.Data.Find<ProductVersion>(s => s.Id == versionId);
            if(version != null)
            {
                version.ApprovalStatus = approvalModel.Status;
                version.ApprovalDate = approvalModel.Date;
                version.ApprovalBy = approvalModel.Manager;
                version.ApprovalComments = approvalModel.Comments;
                if(db.Data.Update(version, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = new string[] { "ApprovalStatus", "ApprovalDate", "ApprovalBy", "ApprovalComments" }
                }))
                {
                    var product = GetProduct(version.ProductId);
                    if(product != null)
                    {
                        messagesService.Notify(
                            new string[] { product.CreatedBy },
                            string.Format("Product version approval change ({0}/{1}). Changed by: {2}", product.Type, product.Name, version.ApprovalBy),
                            DateTime.Now);
                    }
                }

            }
        }

        public void SendDeleteProductRequest(Guid id, string message)
        {
            var product = GetProduct(id);
            if (product != null)
            {
                db.Data.Insert(new DeleteProductRequest()
                {
                    ProductId = id,
                    Message = message
                });

                product.HasDeleteRequest = true;
                db.Data.Update(product, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = new string[] { "HasDeleteRequest" }
                });

                //find and notify user direct managers
                var managedUserGroup = usersDb.Data.Find<ManagedUsersGroup>(s => s.Users.Contains(product.CreatedBy));
                if(managedUserGroup != null && managedUserGroup.Managers != null && managedUserGroup.Managers.Any())
                {
                    messagesService.Notify(
                        managedUserGroup.Managers.Concat(new string[] { product.CreatedBy }).ToArray(),
                        string.Format("@{0} sent a product delete request ({0}/{1})", product.CreatedBy, product.Type, product.Name),
                        DateTime.Now);
                }
            }
        }

        public bool PositiveDeleteProduct(Guid id, string confirmedBy)
        {
            var product = db.Data.Find<Product>(s => s.Id == id);
            if (product != null)
            {
                //actual product delete
                if (db.Data.Update(
                    new DeleteProductRequest() { ProductId = id, ConfirmedBy = confirmedBy },
                    new UpdateFilter() { Behavior = UpdateFilterBehavior.Target, Properties = new string[] { "ConfirmedBy" } } ))
                {
                    db.Data.Delete(new Product() { Id = id });

                    //send notification to user
                    messagesService.Notify(
                        new string[] { product.CreatedBy },
                        string.Format("Your product delete request has been confirmed ({0}/{1})", product.Type, product.Name),
                        DateTime.Now);
                }
            }

            return false;
        }

        public Product GetProduct(Guid id)
        {
            return db.Data.Find<Product>(s => s.Id == id);
        }

        public List<Product> GetProducts()
        {
            return db.Data.Select<Product>().ToList();
        }

        public List<Product> GetProducts(string userId)
        {
            return db.Data.Query<Product>(s => s.CreatedBy == userId).ToList();
        }

        public List<Product> GetProductsByManager(string managerId)
        {
            var users = usersDb.Data.Query<ManagedUsersGroup>(s => s.Managers != null && s.Managers.Contains(managerId)).SelectMany(s => s.Users).ToList();
            if (users != null && users.Any())
            {
                return db.Data.Query<Product>(s => users.Contains(s.CreatedBy)).ToList();
            }

            return null;
        }

        public void UpdateProduct(Product product, string[] fields = null)
        {
            if(fields == null)
            {
                db.Data.Update(product);
            }
            else
            {
                db.Data.Update(product, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = fields
                });
            }
        }
        public void UpdateProductExcept(Product product, string[] fields)
        {
            db.Data.Update(product, new UpdateFilter()
            {
                Behavior = UpdateFilterBehavior.Skip,
                Properties = fields
            });
        }

        public List<ProductApprovalViewModel> GetApprovalStatusList(string userId)
        {
            var products = GetProducts(userId);
            if(products != null && products.Any())
            {
                var ids = products.Select(p => p.Id.ToString()).ToList();
                return db.Data.Query<ProductVersion>(s => ids.Contains(s.ProductId.ToString()))
                    .OrderByDescending(o => o.Date)
                    .Select(s => new ProductApprovalViewModel() {
                        ProductId = s.ProductId,
                        ApprovalStatus = s.ApprovalStatus }).ToList();
            }

            return null;
        }

        public List<ProductApprovalViewModel> GetApprovalStatusListByManager(string managerId)
        {
            var users = usersDb.Data.Query<ManagedUsersGroup>(s => s.Managers != null && s.Managers.Contains(managerId)).SelectMany(s => s.Users).ToList();
            if (users != null && users.Any())
            {
                var products = GetProductsByManager(managerId);
                if (products != null && products.Any())
                {
                    var ids = products.Select(p => p.Id.ToString()).ToList();
                    return db.Data.Query<ProductVersion>(s => ids.Contains(s.ProductId.ToString()))
                        .OrderByDescending(o => o.Date)
                        .Select(s => new ProductApprovalViewModel()
                        {
                            ProductId = s.ProductId,
                            ApprovalStatus = s.ApprovalStatus
                        }).ToList();
                }
            }

            return null;
        }
    }
}