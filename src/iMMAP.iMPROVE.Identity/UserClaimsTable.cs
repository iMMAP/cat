using Xod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class UserClaimsTable
    {
        XodContext database;

        public UserClaimsTable(XodContext identityDbContext)
        {
            this.database = identityDbContext;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null && user.Claims != null && user.Claims.Any())
            {
                foreach (var claim in user.Claims)
                    claims.AddClaim(new Claim(claim.Type, claim.Value));
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public void Delete(string userId)
        {
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null && user.Claims != null && user.Claims.Any())
            {
                user.Claims = null;
                this.database.Update<IdentityUser>(user);
            }
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public void Insert(Claim userClaim, string userId)
        {
            IdentityUser user = this.database.Select<IdentityUser>().FirstOrDefault(s => s.Id == userId);
            if (user != null)
            {
                if (user.Claims == null)
                    user.Claims = new List<UserClaim>();

                user.Claims.Add(new UserClaim()
                    {
                        Type = userClaim.Type,
                        Value = userClaim.Value
                    });
                this.database.Update<IdentityUser>(user);
            }
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public void Delete(IdentityUser user, Claim claim)
        {
            if (user == null || claim == null)
                return;

            if (user.Claims == null)
                user.Claims = new List<UserClaim>();

            user.Claims.Add(new UserClaim()
                {
                    Type = claim.Type,
                    Value = claim.Value
                });

            this.database.Update<IdentityUser>(user);
        }
    }
}
