using Xod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class RoleTable
    {
        XodContext database;
        
        public RoleTable(XodContext identityDbContext)
        {
            this.database = identityDbContext;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public void Delete(string roleId)
        {
            IdentityRole role = database.Select<IdentityRole>().FirstOrDefault(s => s.Id == roleId);
            Delete(role);
        }

        public void Delete(IdentityRole role)
        {
            database.Delete<IdentityRole>(role);
        }


        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public void Insert(IdentityRole role)
        {
            database.Insert<IdentityRole>(role);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(string roleId)
        {
            IdentityRole role = database.Select<IdentityRole>().FirstOrDefault(s => s.Id == roleId);
            if (role != null)
                return role.Name;
            else
                return null;
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            IdentityRole role = database.Select<IdentityRole>().FirstOrDefault(s => s.Name == roleName);
            if (role != null)
                return role.Id;
            else
                return null;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(string roleId)
        {
            return database.Select<IdentityRole>().FirstOrDefault(s => s.Id == roleId);
        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            return database.Select<IdentityRole>().FirstOrDefault(s => s.Name == roleName);
        }

        public void Update(IdentityRole role)
        {
            database.Update<IdentityRole>(role);
        }
    }
}
