using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPACT.Models
{
    public class ManagedUsersGroup
    {
        public int Id { get; set; }
        public List<string> Managers { get; set; }
        public List<string> Users { get; set; }
    }
}
