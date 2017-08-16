using System.Web;

namespace iMMAP.iMPACT.Services.Helpers
{
    public class IOHelper
    {
        public static string GetDatabasePath()
        {
            string relPath = System.Configuration.ConfigurationManager.AppSettings["Database"];
            string absPath = HttpContext.Current.Server.MapPath(relPath);
            return absPath;
        }

        public static string GetUsersDatabasePath()
        {
            string relPath = System.Configuration.ConfigurationManager.AppSettings["UsersDatabase"];
            string absPath = HttpContext.Current.Server.MapPath(relPath);
            return absPath;
        }
    }
}