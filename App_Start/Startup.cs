using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Jindo_Capstone.Workers;
using Hangfire;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(Jindo_Capstone.App_Start.Startup))]

namespace Jindo_Capstone.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard("/jobs");

            /* requires paid azure webssite to keep the job running in the background
             * References: https://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html#azure-web-applications
             */
            //Executes task bi-weekly on wensday at noon 
        //RecurringJob.AddOrUpdate("SendTextMsgJob", () => SendTextMessageJob.Execute(), "* 12 */15 * 3" );
            //RecurringJob.AddOrUpdate("SendTextMsgJob", () => SendTextMessageJob.Execute(), "0 * * ? * *");

        }
        public void Configure(IAppBuilder app) {

        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire")
                .UseColouredConsoleLogProvider();
            yield return new BackgroundJobServer(); 
        }
    }
}
