using System.Web;
using System.Web.Optimization;

namespace iMMAP.iMPROVE
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Themes/base/bootstrap.css",
                      "~/Themes/base/site.css"));

            bundles.Add(new StyleBundle("~/dashboard/css").Include(
                      "~/Themes/base/bootstrap.css",
                      "~/Themes/dashboard/css/AdminLTE.css",
                      "~/Themes/dashboard/css/skins/_all-skins.css",
                      "~/Themes/dashboard/default.css"));

            bundles.Add(new ScriptBundle("~/core/js").Include(
                "~/Scripts/core/dashboard.shared.js"));

            bundles.Add(new ScriptBundle("~/dashboard/js").Include(
                "~/Themes/dashboard/js/app.js"));

            bundles.Add(new ScriptBundle("~/signalr/js").Include(
                "~/Scripts/jquery.signalR-2.2.2.js"));

            AddToBundles(ref bundles, "~/Themes/dashboard/plugins", "plugin");

#if (!DEBUG)
            BundleTable.EnableOptimizations = true;
#endif
        }

        private static void AddToBundles(ref BundleCollection bundles, string path, string prefex)
        {
            foreach (var dir in System.IO.Directory.GetDirectories(
                HttpContext.Current.Server.MapPath(path)))
            {
                string dirName = new System.IO.DirectoryInfo(dir).Name;

                if (System.IO.Directory.Exists(dir + "/js"))
                    bundles.Add(new ScriptBundle(string.Format("~/bundles/{0}js/{1}", prefex, dirName)).Include(
                        string.Format("{0}/{1}/js/*.js", path, dirName)));

                if (System.IO.Directory.Exists(dir + "/css"))
                    bundles.Add(new StyleBundle(string.Format("~/bundles/{0}css/{1}", prefex, dirName)).Include(
                        string.Format("{0}/{1}/css/*.css", path, dirName)));
            }
        }
    }
}
