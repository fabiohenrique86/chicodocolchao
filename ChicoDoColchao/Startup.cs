using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChicoDoColchao.Startup))]
namespace ChicoDoColchao
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ConfigureAuth(app);
        }
    }
}
