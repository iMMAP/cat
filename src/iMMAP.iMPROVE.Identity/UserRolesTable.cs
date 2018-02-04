using Xod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class UserRolesTable
    {
        XodContext database;

        public UserRolesTable(XodContext identityDbContext)
        {
            this.database = identityDbContext;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
            {
                return this.database.Select<IdentityRole>().Where(s => user.Roles != null && user.Roles.Contains(s.Id)).Select(s => s.Name).ToList();
            }

            return null;
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(string userId)
        {
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
            {
                user.Roles = null;
                this.database.Update<IdentityUser>(user);
            }
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public void Insert(IdentityUser user, string roleId)
        {
            if (user == null || string.IsNullOrEmpty(roleId))
                return;

            if (user.Roles == null)
                user.Roles = new List<string>();

            if (!user.Roles.Contains(roleId))
            {
                user.Roles.Add(roleId);
                this.database.Update<IdentityUser>(user);
            }
        }
    }
}
