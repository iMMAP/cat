using iMMAP.iMPACT.Identity;
using iMMAP.iMPACT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xod;

namespace iMMAP.iMPACT.Services
{
    public interface IProductsService
    {
        List<Product> GetProducts();
        Product GetProduct(Guid id);
        Product AddProduct(Product product);
        void UpdateProduct(Product product, string[] fields = null);
        void SendDeleteProductRequest(Guid id, string message);
        bool PositiveDeleteProduct(Guid id, string confirmedBy);
        void ChangeProductAproval(string versionId, ProductApprovalModel status);
    }

    public class ProductsService: IProductsService
    {
        private IDataService db = null;
        private IUsersDataService usersDb = null;
        private INotificationService notificationService = null;

        public ProductsService(IDataService db, IUsersDataService usersDb, INotificationService notificationService)
        {
            this.db = db;
            this.usersDb = usersDb;
            this.notificationService = notificationService;
        }

        public Product AddProduct(Product product)
        {
            return (Product)db.Data.Insert(product);
        }

        public void ChangeProductAproval(string versionId, ProductApprovalModel approvalModel)
        {
            var version = db.Data.Find<ProductVersion>(s => s.Id == versionId);
            if(version != null)
            {
                version.ApprovalStatus = approvalModel.Status;
                version.ApprovalUpdateDate = approvalModel.Date;
                version.ApprovalBy = approvalModel.Manager;
                version.ApprovalComments = approvalModel.Comments;
                db.Data.Update(version, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = new string[] { "ApprovalStatus", "ApprovalUpdateDate", "ApprovalManager", "ApprovalComments" }
                });
            }
        }

        public void SendDeleteProductRequest(Guid id, string message)
        {
            var product = db.Data.Find<Product>(s => s.Id == id);
            if(product != null)
            {
                db.Data.Insert(new DeleteProductRequest()
                {
                    ProductId = id,
                    Message = message
                });

                //find and notify user direct managers
                var managedUsersGroup = usersDb.Data.Find<ManagedUsersGroup>(s => s.Users.Contains(product.CreatedBy));
                if(managedUsersGroup != null)
                {
                    var deleteNotification = new Notification()
                    {
                        Sent = DateTime.Now,
                        Body = string.Format("{0} sent a product delete request ({0}/{1})", product.CreatedBy, product.Type, product.Name),
                        To = managedUsersGroup.Managers
                    };
                    notificationService.Notify(deleteNotification);
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

                    //send notification to user
                    var deleteNotification = new Notification()
                    {
                        Sent = DateTime.Now,
                        Body = string.Format("Your product delete request is confirmed ({0}/{1})", product.Type, product.Name),
                        To = new List<string> { product.CreatedBy }
                    };
                    notificationService.Notify(deleteNotification);
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

        public List<Product> GetProducts(Guid userId)
        {
            return db.Data.Query<Product>(s => s.CreatedBy == userId).ToList();
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
    }
}