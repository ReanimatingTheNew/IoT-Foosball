﻿using Microsoft.Owin;
using Owin;
using Referee.Web;

[assembly: OwinStartup(typeof (Startup))]

namespace Referee.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}