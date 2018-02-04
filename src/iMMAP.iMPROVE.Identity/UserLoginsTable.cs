using Xod;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class UserLoginsTable
    {
        XodContext database;

        public UserLoginsTable(XodContext identityDbContext)
        {
            this.database = identityDbContext;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public void Delete(IdentityUser user, UserLoginInfo login)
        {
            if (user == null || login == null)
                return;

            if (user.Logins == null)
                user.Logins = new List<UserLogin>();

            user.Logins.Add(new UserLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            });

            this.database.Update<IdentityUser>(user);
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(string userId)
        {
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null && user.Logins != null && user.Logins.Any())
            {
                user.Logins = null;
                this.database.Update<IdentityUser>(user);
            }
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public void Insert(IdentityUser user, UserLoginInfo login)
        {
            if (user == null || login == null)
                return;

            UserLogin userLogin = new UserLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            if (user.Logins == null)
                user.Logins = new List<UserLogin>();

            user.Logins.Add(userLogin);

            this.database.Update<IdentityUser>(user);
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Logins != null &&
                s.Logins.Where(s2 =>
                    s2.LoginProvider == userLogin.LoginProvider &&
                    s2.ProviderKey == userLogin.ProviderKey).Any());

            return user.UserName;
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null && user.Logins != null && user.Logins.Any())
            {
                foreach (var login in user.Logins)
                    logins.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            return logins;
        }
    }
}
