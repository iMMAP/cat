using iMMAP.iMPACT.Models.ViewModels;
using System.Collections.Generic;

namespace iMMAP.iMPACT.Models.ViewModels
{
    public class ProductsViewModel
    {
        public UserViewModel User { get; set; }
        public List<Product> Products { get; set; }
    }
}
