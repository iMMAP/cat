using Xod;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class RoleStore : IRoleStore<IdentityRole>
    {
        private RoleTable roleTable;
        XodContext database;
        
        public RoleStore(string dbFile, string password = null)
        {
            this.database = new XodContext(dbFile, password);
            roleTable = new RoleTable(this.database);
        }

        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            roleTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            roleTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<IdentityRole> FindByIdAsync(string roleId)
        {
            IdentityRole result = roleTable.GetRoleById(roleId);

            return Task.FromResult<IdentityRole>(result);
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            IdentityRole result = roleTable.GetRoleByName(roleName);

            return Task.FromResult<IdentityRole>(result);
        }

        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("user");
            }

            roleTable.Update(role);

            return Task.FromResult<Object>(null);
        }

        public void Dispose()
        {
            if (this.database != null)
            {
                this.database.Dispose();
                this.database = null;
            }
        }
    }
}
