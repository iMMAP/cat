using Xod;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPACT.Identity
{
    public class IdentityUser : IUser
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        [PrimaryKey]
        public string Id { get; set; }
        [UniqueKey]
        public string UserName { get; set; }
        //public string iMMAP.iMPACT { get; set; }
        //public string StorageCode { get; set; }
        public string[] Domains { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public List<string> Roles { get; set; }
        [Children]
        public List<UserClaim> Claims { get; set; }
        [Children]
        public List<UserLogin> Logins { get; set; }
        [UniqueKey]
        public string Email { get; set; }
        public string FullName { get; set; }
        public UserType Type { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string DefaultLanguage { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime LastAccess { get; set; }
        public bool Suspended { get; set; }
    }

    public enum Gender
    {
        Male, Female, Undisclosed
    }

    public enum UserType
    {
        Standard, Professional
    }
}
