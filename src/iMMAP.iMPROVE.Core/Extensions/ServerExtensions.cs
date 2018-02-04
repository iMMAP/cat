using System.Web;

namespace iMMAP.iMPROVE.Core.Extensions
{
    public static class ServerExtension
    {
        public static string RelativePath(this HttpServerUtilityBase server, string path)
        {
            return path.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], "/").Replace(@"\", "/");
        }
    }
}
