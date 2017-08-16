using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iMMAP.iMPACT.Startup))]
namespace iMMAP.iMPACT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
