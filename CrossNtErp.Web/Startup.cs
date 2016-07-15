using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrossNtErp.Web.Startup))]
namespace CrossNtErp.Web {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
