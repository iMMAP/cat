using Xod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Identity
{
    public class UserClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        [ParentKey("UserId")]
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
    }
}
