using Microsoft.Owin;
using Owin;
using ZachsPetBoarding.seeds;

[assembly: OwinStartupAttribute(typeof(ZachsPetBoarding.Startup))]
namespace ZachsPetBoarding
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            AdminSeed adminSeed = new AdminSeed();
            adminSeed.SeedAdmin();
            EmailServiceCredentials.PopulateEmailCredentialsFromAppConfig();
        }
        

    }
    
}
