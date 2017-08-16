using iMMAP.iMPACT.Identity;
using Microsoft.AspNet.Identity;

namespace iMMAP.iMPACT.Services
{
    public interface IUsersService
    {
        UserManager<IdentityUser> UserManager { get; }
    }

    public class UsersService: IUsersService
    {
        public UserManager<IdentityUser> UserManager { get; private set; }

        public UsersService()
        {
            this.UserManager = new UserManager<IdentityUser>(new UserStore(Helpers.IOHelper.GetUsersDatabasePath()));
        }
    }
}
