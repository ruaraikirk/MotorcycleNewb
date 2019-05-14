using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MotorcycleNewb.Startup))]
namespace MotorcycleNewb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
