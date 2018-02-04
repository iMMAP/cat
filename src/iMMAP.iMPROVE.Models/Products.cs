using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public Months Month { get; set; }
        public ProductType Type { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Preview { get; set; }

        public string CreatedBy { get; set; }

        [Xod.ParentKey("ProgramId")]
        public RegionalOfficeProgram Program { get; set; }
        public Guid ProgramId { get; set; }

        public Guid ActiveVersionProductId { get; set; }
        public bool Published { get; set; }
        public bool Public { get; set; }

        public ReferenceThematic ReferenceThematic { get; set; }
        public AdminLevel Level { get; set; }
        public string Governorates { get; set; }
        public string Districts { get; set; }

        [Xod.Children]
        public List<ProductVersion> Versions { get; set; }
        public bool HasDeleteRequest { get; set; }
    }

    public enum Months
    {
        January, February, March, April, May, June, July, August, Septemper, October, November, December
    }

    public enum AdminLevel
    {
        Governorate, District
    }

    public enum ReferenceThematic
    {
        Reference, Thematic
    }

    public class ProductVersion
    {
        public Guid Id { get; set; }

        [Xod.Property(AutoNumber = true)]
        public int SerialNumber { get; set; }
        public string VerionNumber { get; set; }
        public ChangesType Changes { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public string ApprovalBy { get; set; }
        public ProductApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovalComments { get; set; }

        [Xod.ParentKey("ProductId")]
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
    }

    public enum ChangesType
    {
        InisialVersion, DatasetUpdate, MajorDesign, MinerDesign
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
        Review, Directions, Approved, Rejected
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
