using Xod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class UserTable
    {
        XodContext identityDbContext;
        
        public UserTable(XodContext identityDbContext)
        {
            this.identityDbContext = identityDbContext;
        }

        public string GetUserName(string userId)
        {
            IdentityUser user = identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
                return user.UserName;
            else
                return null;
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            IdentityUser user = identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.UserName == userName);
            if (user != null)
                return user.Id;
            else
                return null;
        }

        /// <summary>
        /// Returns an IdentityUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public IdentityUser GetUserById(string userId)
        {
            return identityDbContext.Query<IdentityUser>(s => s.Id == userId).FirstOrDefault();
        }

        /// <summary>
        /// Returns a list of IdentityUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<IdentityUser> GetUserByName(string userName)
        {
            return identityDbContext.Query<IdentityUser>(s => s.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            IdentityUser user = identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
                return user.PasswordHash;
            else
                return null;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            IdentityUser user = identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
            {
                user.PasswordHash = passwordHash;
                if (identityDbContext.Update<IdentityUser>(user))
                    return 1;
            }
            return 0;
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            IdentityUser user = identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
                return user.SecurityStamp;
            else
                return null;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(IdentityUser user)
        {
            if (identityDbContext.Insert<IdentityUser>(user) != null)
                return 1;
            return 0;
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
            IdentityUser user = identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            return Delete(user);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(IdentityUser user)
        {
            if (identityDbContext.Delete<IdentityUser>(user))
                return 1;
            return 0;
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(IdentityUser user)
        {
            if (identityDbContext.Update<IdentityUser>(user, new UpdateFilter() {
                Behavior = UpdateFilterBehavior.Skip,
                Properties = new string[] { "Email", "iMMAP.iMPROVE" }
            }))
                return 1;
            return 0;
        }

        public IdentityUser GetUserByEmail(string email)
        {
            return identityDbContext.Select<IdentityUser>().FirstOrDefault(s => s.Email == email);
        }

        public int SetEmail(IdentityUser user, string email)
        {
            user.Email = email;
            if (identityDbContext.Update<IdentityUser>(user, new UpdateFilter()
            {
                Behavior = UpdateFilterBehavior.Target,
                Properties = new string[] { "Email" }
            }))
                return 1;
            return 0;
        }

        public int SetEmailConfirmed(IdentityUser user, bool emailConfirmed)
        {
            user.EmailConfirmed = emailConfirmed;
            if (identityDbContext.Update<IdentityUser>(user, new UpdateFilter()
                {
                    Behavior = UpdateFilterBehavior.Target,
                    Properties = new string[] { "EmailConfirmed" }
                }))
                return 1;
            return 0;
        }
    }
}
