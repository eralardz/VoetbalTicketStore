using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VoetbalTicketStore.Startup))]
namespace VoetbalTicketStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
