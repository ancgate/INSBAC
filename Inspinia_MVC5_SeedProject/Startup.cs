using Microsoft.Owin;
using Owin;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using phoebe.Models;

[assembly: OwinStartupAttribute(typeof(phoebe.Startup))]
namespace phoebe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
