using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Jindo_Capstone.Workers;
[assembly: OwinStartup(typeof(Jindo_Capstone.App_Start.Startup1))]

namespace Jindo_Capstone.App_Start
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            SendTextMessageJob s = new SendTextMessageJob();
            s.Execute();

        }
        public void Configure(IAppBuilder app) {

        }
    }
}
