using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PIMS.Startup))]
namespace PIMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
