using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iMMAP.iMPROVE.Startup))]
namespace iMMAP.iMPROVE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
