using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPACT.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public Months Month { get; set; }
        public ProductType Type { get; set; }
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public int ProgramId { get; set; }

        public Guid ActiveVersionProductId { get; set; }
        public bool Published { get; set; }
        public bool Public { get; set; }
    }

    public enum Months
    {
        Jen, Feb, Mar, Apr, Jun, Jul, Aug, Sep, Nov, Dec
    }

    public class ProductVersion
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string VerionNumber { get; set; }
        public DateTime Updated { get; set; }
        public string Description { get; set; }

        public string ApprovalBy { get; set; }
        public ProductApprovalStatus ApprovalStatus { get; set; }
        public DateTime ApprovalUpdateDate { get; set; }
        public string ApprovalComments { get; set; }
    }

    public class ProductApprovalModel
    {
        public string Manager { get; set; }
        public ProductApprovalStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
    }

    public enum ProductApprovalStatus
    {
        Review, NewVersionReview, Approved, Rejected
    }

    public enum ProductType
    {
        Map, Dashboard, Tool, Report, Infograph, Other
    }

    public class DeleteProductRequest
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public string Message { get; set; }
        public string ConfirmedBy { get; set; }
        public bool Confirmed { get { return this.ConfirmedBy != null; } }
    }
}
