namespace iMMAP.iMPROVE.Core.Services
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
        IIOService ioService = null;
        public Xod.XodContext Data { get; private set; }

        public DataService(IIOService ioService)
        {
            this.ioService = ioService;
            this.Data = new Xod.XodContext(ioService.GetDatabasePath(), null, new Xod.DatabaseOptions()
            {
                LazyLoadParent = false
            });
        }
    }

    public class UsersDataService : IUsersDataService
    {
        IIOService ioService = null;
        public Xod.XodContext Data { get; private set; }

        public UsersDataService(IIOService ioService)
        {
            this.ioService = ioService;
            this.Data = new Xod.XodContext(ioService.GetUsersDatabasePath());
        }
    }
}
