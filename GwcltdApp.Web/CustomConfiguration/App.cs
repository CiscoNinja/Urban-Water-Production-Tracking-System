using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.CustomConfiguration
{
    public class App
    {
        public static ApplicationConfiguration Configuration { get; set; }

        static App()
        {
            // Create an instance of the class with default provider
            Configuration = new ApplicationConfiguration();
            Configuration.Initialize();
        }
    }
}