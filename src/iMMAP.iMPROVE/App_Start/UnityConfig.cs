using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using iMMAP.iMPROVE.Core.Services;
using iMMAP.iMPROVE.Hubs;

namespace iMMAP.iMPROVE
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IIOService, IOService>();
            container.RegisterType<IDataService, DataService>();
            container.RegisterType<IUsersDataService, UsersDataService>();
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<IMessagesService, MessagesService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProductsService, ProductsService>();
            container.RegisterType<IActivitiesService, ActivitiesService>();
            container.RegisterType<IProgramsService, ProgramsService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}