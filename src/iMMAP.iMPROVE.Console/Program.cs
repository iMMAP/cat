using iMMAP.iMPROVE.Identity;
using iMMAP.iMPROVE.Models;
using iMMAP.iMPROVE.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;
using Unity.Mvc5;
using System.Collections.Generic;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Annotations;

namespace iMMAP.iMPROVE.Console2
{
    class Program
    {
        public static UnityContainer Container { get; internal set; }

        static void Main(string[] args)
        {
            RegisterComponents();
            Seed();
            //ReadAnnot();
            //ReadPdf();
        }

        private static void ReadPdf()
        {
            string filename = @"P:\iMPROVE\res\MAP3.pdf";
            PdfDocument document = PdfReader.Open(filename);
            PdfPage page = document.Pages[0];
            foreach (PdfAnnotation an in page.Annotations)
            {
                //dynamic r = an.Rectangle;
            }
        }

        public static void Seed()
        {
            IUsersService usersService = Program.Container.Resolve<IUsersService>();
            IUsersDataService usersDataService = Program.Container.Resolve<IUsersDataService>();
            IProgramsService programsService = Program.Container.Resolve<IProgramsService>();
            IProductsService productService = Program.Container.Resolve<IProductsService>();

            var office = new RegionalOffice()
            {
                Organization = Organization.UNICEF,
                Region = "Aden",
                Email = "office@org.com",
                Phone = "2983742",
                Country = "Yemen",
                Programs = new List<RegionalOfficeProgram>
                {
                    new RegionalOfficeProgram()
                    {
                        Name = "Education",
                        OfficerName = "Abc",
                        OfficerEmail = "officer@org.com",
                        OfficerPhone = "2387483"
                    }
                }
            };
            programsService.AddRegionalOffice(office);

            var prog = office.Programs[0].Id;

            usersDataService.Data.Insert(new IdentityRole() { Id = "admin", Name = "Administrator" });
            usersDataService.Data.Insert(new IdentityRole() { Id = "manager", Name = "Manager" });
            usersDataService.Data.Insert(new IdentityRole() { Id = "user", Name = "User" });

            usersService.UserManager.CreateAsync(new IdentityUser()
            {
                UserName = "msallam",
                FullName = "Mohammed H. Sallam",
                Email = "msallam@immap.org",
                Type = UserType.User,
                OfficeId = office.Id,
                Roles = new List<string> { "user" }
            }, "password");
            usersService.UserManager.CreateAsync(new IdentityUser()
            {
                UserName = "manager",
                FullName = "Manager",
                Email = "manager@immap.org",
                Type = UserType.Manager,
                Roles = new List<string> { "manager" }
            }, "password");
            usersService.UserManager.CreateAsync(new IdentityUser()
            {
                UserName = "admin",
                FullName = "Administrator",
                Email = "admin@immap.org",
                Type = UserType.Administrator,
                Roles = new List<string> { "admin" }

            }, "password");

            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    var prod = new Product()
                    {
                        Name = ((ProductType)j).ToString() + " " + (i + 1),
                        Month = Months.August,
                        Type = ((ProductType)j),
                        Created = DateTime.Now.AddDays(-(i + 1)),
                        CreatedBy = "msallam",
                        ProgramId = prog,
                        Public = true,
                        Description = ((ProductType)j).ToString() + " product " + (j + 1) + "...",
                    };
                    productService.AddProduct(prod);
                }
            }

            usersDataService.Data.Insert(new ManagedUsersGroup()
            {
                Managers = new List<string> { "manager", "admin" },
                Users = new List<string> { "msallam" }
            });

            var prod2 = new Product()
            {
                Name = "Map Product",
                Month = Months.Septemper,
                Type = ProductType.Map,
                Created = DateTime.Now.AddDays(-3),
                CreatedBy = "msallam",
                ProgramId = prog,
                Public = true,
                Versions = new List<ProductVersion>
                {
                    new ProductVersion()
                    {
                        Changes = ChangesType.InisialVersion,
                        Description = "Initial product version...",
                        Date = DateTime.Now.AddDays(-3),
                        ApprovalDate = DateTime.Now,
                        ApprovalStatus = ProductApprovalStatus.Approved,
                        ApprovalBy = "manager"
                    },
                    new ProductVersion()
                    {
                        Id = Guid.NewGuid(),
                        Changes = ChangesType.MajorDesign,
                        Description = "Major update in map design",
                        Date = DateTime.Now.AddDays(-2),
                        ApprovalStatus = ProductApprovalStatus.Review,
                    }
                }
            };
            productService.AddProduct(prod2);
        }

        public static void RegisterComponents()
        {
            Program.Container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            Program.Container.RegisterType<IIOService, IOConsoleService>();
            Program.Container.RegisterType<IDataService, DataService>();
            Program.Container.RegisterType<IUsersDataService, UsersDataService>();
            Program.Container.RegisterType<IUsersService, UsersService>();
            Program.Container.RegisterType<IMessagesService, MessagesService>();
            Program.Container.RegisterType<IProductsService, ProductsService>();
            Program.Container.RegisterType<IActivitiesService, ActivitiesService>();
            Program.Container.RegisterType<IProgramsService, ProgramsService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(Program.Container));
        }

        public class IOConsoleService : IIOService
        {
            public string GetDatabasePath()
            {
                return @"P:\iMPROVE\src\iMMAP.iMPACT\Data\DB\data.xod";
            }

            public string GetUniqueFilePath(string filePath)
            {
                throw new NotImplementedException();
            }

            public string GetUsersDatabasePath()
            {
                return @"P:\iMPROVE\src\iMMAP.iMPACT\Data\Users\data.xod";
            }
        }

    }
}
