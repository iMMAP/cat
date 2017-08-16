using iMMAP.iMPACT.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPACT.Services
{
    public interface IDataService
    {
        Xod.XodContext Data { get; }
    }

    public interface IUsersDataService
    {
        Xod.XodContext Data { get; }
    }

    public class DataService : IDataService
    {
        public Xod.XodContext Data { get; private set; }

        public DataService()
        {
            this.Data = new Xod.XodContext(IOHelper.GetDatabasePath());
        }
    }

    public class UsersDataService : IUsersDataService
    {
        public Xod.XodContext Data { get; private set; }

        public UsersDataService()
        {
            this.Data = new Xod.XodContext(IOHelper.GetUsersDatabasePath());
        }
    }
}
