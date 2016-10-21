﻿using System.Data.Common;
using Microsoft.Owin;
using Owin;
using Hangfire;

[assembly: OwinStartup(typeof(HangfireClient.Startup))]

namespace HangfireClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.UseHangfireDashboard();
        }
    }
}
