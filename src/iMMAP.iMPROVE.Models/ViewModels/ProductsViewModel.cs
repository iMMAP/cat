using iMMAP.iMPROVE.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace iMMAP.iMPROVE.Models.ViewModels
{
    public class ProductsViewModel
    {
        public UserViewModel User { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductApprovalViewModel> ApprovalStatusList { get; set; }
    }

    public class ProductApprovalViewModel
    {
        public Guid ProductId { get; set; }
        public ProductApprovalStatus ApprovalStatus { get; set; }
    }

    public class ProductVersionViewModel
    {
        public bool NewMode { get; set; }
        public Guid ProductId { get; set; }
        public ChangesType Changes { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class EditProductViewModel
    {
        public bool EditMode { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public Product Product { get; set; }
        public RegionalOffice Office { get; set; }
        public RegionalOfficeProgram Program { get; set; }
        public List<ProductVersion> Versions { get; set; }
    }

    public class ReviewProductViewModel
    {
        public UserViewModel CreatedBy { get; set; }
        public Product Product { get; set; }
        public RegionalOffice Office { get; set; }
        public RegionalOfficeProgram Program { get; set; }
        public List<ProductVersion> Versions { get; set; }
    }
}
