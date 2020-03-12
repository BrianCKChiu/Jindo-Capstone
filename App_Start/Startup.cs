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
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //SendTextMessageJob s = new SendTextMessageJob();
            //s.Execute();
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard("/jobs");
            //BackgroundJob.Enqueue(() => s.Execute());
            //Executes task bi-weekly on wensday at noon 
            //RecurringJob.AddOrUpdate("Send-TextMsg", () => SendTextMessageJob.Execute(), "* 12 */15 * 3" );
            RecurringJob.AddOrUpdate("Send-TextMsg", () => SendTextMessageJob.Execute(), Cron.Hourly);
        }
        public void Configure(IAppBuilder app) {

        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire");
            yield return new BackgroundJobServer(); 
        }
    }
}
