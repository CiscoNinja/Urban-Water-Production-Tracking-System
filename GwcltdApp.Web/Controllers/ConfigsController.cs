using GwcltdApp.Web.CustomConfiguration;
using GwcltdApp.Web.CustomConfiguration.ConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Westwind.Utilities;
using Westwind.Utilities.Configuration;

namespace GwcltdApp.Web.Controllers
{
    public class ConfigsController : Controller
    {
        private const string STR_SUPERSECRET = "SUPERSECRET";

        /// <summary>
        /// Local instance of the configuration class that we'll end up binding to
        /// </summary>
        public ApplicationConfiguration AppConfig = null;


        /// <summary>
        /// Just display Configuration data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(ConfigViewModel model)
        {
            if (model == null)
                model = new ConfigViewModel();

            this.LoadAppConfiguration(model);

            model.Configuration = this.AppConfig;
            //model.ErrorMessage = "Hello World";
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Save(ConfigViewModel model)
        {
            bool isSave = !string.IsNullOrEmpty(Request.Form["btnSubmit"]);
            if (!isSave)
                return Index(model);

            this.LoadAppConfiguration(model);

            if (this.AppConfig == null)
            {
                // Load the raw Config object without loading anything
                AppConfig = new ApplicationConfiguration();
            }

            // Read all Formvars directly into the AppConfig
            WebUtils.FormVarsToObject(this.AppConfig, "Configuration.");
            
                if (this.AppConfig.Write())
                    model.ShowMessage("Keys have been written to the database .<hr>");
                else
                    model.ShowError("Writing the keys to the database failed <hr>" +
                                   "The connecton string or table namea and PK are incorrect. Use the database settings in AppConfiguration " +
                                   "to set up a connection string, then create a table called Config and add a text field called ConfigData and a PK field.");
            

            model.Configuration = this.AppConfig;
            return View("Index", model);
        }


        private void LoadAppConfiguration(ConfigViewModel model)
        {
                var provider = new SqlServerConfigurationProvider<ApplicationConfiguration>()
                {
                    ConnectionString = "Data Source=.\\SQLExpress;Initial Catalog=GwcltdAppDB6;Integrated Security=SSPI; MultipleActiveResultSets=true",
                    Tablename = "ConfigData",
                    Key = 1,
                    PropertiesToEncrypt = "ConnectionString",
                    EncryptionKey = STR_SUPERSECRET
                };

                AppConfig = new ApplicationConfiguration();
                AppConfig.Initialize(provider);

                if (!this.AppConfig.Read())
                    model.ShowError(
                      "Unable to connect to the Database.<hr>" +
                      "This database samle uses the connection string in the configuration settings " +
                      "with a table named 'ConfigData' and a field named 'ConfigData' to hold the " +
                      "configuration settings. If you have a valid connection string you can click " +
                      "on Save Settings to force the table and a single record to be created.<br><br>" +
                      "Note: The table name is parameterized (and you can change it in the default.aspx.cs page), but the field name always defaults to ConfigData.<hr/>" +
                      AppConfig.ErrorMessage);
            
        }
    }
}