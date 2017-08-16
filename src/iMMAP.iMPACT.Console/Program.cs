using iMMAP.iMPACT.Identity;
using iMMAP.iMPACT.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPACT.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Seed();

            //Xod.XodContext db = new Xod.XodContext(@"P:\iMMAP.iMPACT\iMMAP.iMPACT\Data\Users\data.xod", null, new Xod.DatabaseOptions() { InitialCreate = true });
            //db.Insert<IdentityRole>(new IdentityRole()
            //{
            //    Id = "admin",
            //    Name = "Administrator"
            //});
            //db.Insert<IdentityRole>(new IdentityRole()
            //{
            //    Id = "officer",
            //    Name = "Officer"
            //});
            //UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore(@"P:\iMMAP.iMPACT\iMMAP.iMPACT\Data\Users\data.xod"));
            //var user = new IdentityUser()
            //{
            //    UserName = "admin",
            //    FullName = "Administrator",
            //    Email = "mhisallam@outlook.com",
            //    Roles = new List<string> { "admin" }
            //};
            //var result = userManager.CreateAsync(user, "password");
            //user = new IdentityUser()
            //{
            //    UserName = "msallam",
            //    FullName = "Mohammed Hassan Sallam",
            //    Email = "msallam@immap.org",
            //    Roles = new List<string> { "officer" }
            //};
            //result = userManager.CreateAsync(user, "password");
            //Xod.XodContext db = new Xod.XodContext(@"P:\iMMAP.iMPACT\iMMAP.iMPACT\Data\Users", "fedoracore", new Xod.DatabaseOptions()
            //{
            //    InitialCreate = true
            //});
            //db.Insert<iMMAP.iMPACT.Identity.IdentityUser>(new Identity.IdentityUser()
            //{
            //    UserName = "admin",
            //    Email = "mhisallam@outlook.com",
            //    EmailConfirmed = true,
            //    PasswordHash = 
            //});
        }

        public static void Seed()
        {
            Xod.XodContext db = new Xod.XodContext(@"P:\iMMAP.iMPACT\iMMAP.iMPACT\Data\DB\data.xod");
            var office = new RegionalOffice()
            {
                Organization = Organization.UNICEF,
                Region = "Aden"
            };
            db.Insert(office);

            var prog = new RegionalOfficeProgram()
            {
                Name = "Education",
                ParentOfficeId = office.Id
            };
            db.Insert(prog);

            var userId = db.Find<IdentityUser>(s => s.UserName == "msallam").Id;
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    var prod = new Product()
                    {
                        Name = ((ProductType)j).ToString() + " " + (i + 1),
                        Month = Months.Aug,
                        Type = ((ProductType)j),
                        Created = DateTime.Now.AddDays(-(i + 1)),
                        CreatedBy = userId,
                        ProgramId = prog.Id,
                        Public = true,
                        Description = ((ProductType)j).ToString() + " product " + (j + 1) + "...",
                    };
                    db.Insert(prod);

                    var version = new ProductVersion()
                    {
                        ProductId = prod.Id,
                        VerionNumber = "1.0.0",
                        Description = "...",
                        Updated = DateTime.Now,
                        ApprovalBy = "admin",
                        ApprovalUpdateDate = DateTime.Now,
                        ApprovalStatus = ProductApprovalStatus.Approved
                    };
                    db.Insert(version);
                }
            }
        }
    }

}
