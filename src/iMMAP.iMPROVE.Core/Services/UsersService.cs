using System;
using iMMAP.iMPROVE.Identity;
using Microsoft.AspNet.Identity;

namespace iMMAP.iMPROVE.Core.Services
{
    public interface IUsersService
    {
        UserManager<IdentityUser> UserManager { get; }
        //RoleManager<IdentityRole> RoleManager { get; }
        //void CreateRole(string roleName);
    }

    public class UsersService: IUsersService
    {
        IIOService ioService = null;
        public UserManager<IdentityUser> UserManager { get; private set; }
        //public RoleManager<IdentityRole> RoleManager { get; private set; }

        public UsersService(IUsersDataService usersDataService, IIOService ioService)
        {
            this.ioService = ioService;
            string userDbPath = ioService.GetUsersDatabasePath();
            this.UserManager = new UserManager<IdentityUser>(new UserStore(userDbPath));
            //this.RoleManager = new RoleManager<IdentityRole>(new RoleStore(userDbPath));
        }

        //public void CreateRole(string roleName)
        //{
        //    if (string.IsNullOrEmpty(roleName))
        //        return;

        //    RoleManager.Create(new IdentityRole()
        //    {
        //        Name = roleName
        //    });
        //}
    }
}
