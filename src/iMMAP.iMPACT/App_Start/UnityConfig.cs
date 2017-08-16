using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using iMMAP.iMPACT.Services;

namespace iMMAP.iMPACT
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IUsersDataService, UsersDataService>();
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<INotificationService, NotificationService>();
            container.RegisterType<IProductsService, ProductsService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}