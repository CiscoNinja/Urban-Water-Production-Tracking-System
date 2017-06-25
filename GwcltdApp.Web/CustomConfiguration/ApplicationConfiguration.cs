using GwcltdApp.Web.CustomConfiguration.ConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Westwind.Utilities.Configuration;

namespace GwcltdApp.Web.CustomConfiguration
{
    /// <summary>
    /// Application specific config class that holds global configuration values
    /// that are read and optionally persisted into a configuration store.
    /// </summary>    
    public class ApplicationConfiguration : AppConfiguration
    {

        /// <summary>
        /// Override default provider creation by using custom options
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="configData"></param>
        /// <returns></returns>
        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            var provider = new SqlServerConfigurationProvider<ApplicationConfiguration>()
            {
                ConnectionString = "Data Source=.\\SQLExpress;Initial Catalog=GwcltdAppDBF;Integrated Security=SSPI; MultipleActiveResultSets=true",
                Tablename = "ConfigData",
                PropertiesToEncrypt = "ConnectionString",
                EncryptionKey = "secret",
                Key = 1,
                ProviderName = "System.Data.SqlClient"
            };

            return provider;
        }

        // Define properties and defaults

        public ApplicationConfiguration()
        {
            ApplicationTitle = "Ghana Water Company Ltd.";
            ApplicationSubTitle = "Monthly Production Report";
            ApplicationCookieName = "_ApplicationId";
        }

        /// <summary>
        /// The main title of the Weblog
        /// </summary>
        public string ApplicationTitle { get; set; }


        /// <summary>
        /// The subtitle for the Web Log displayed on the banner
        /// </summary>
        public string ApplicationSubTitle { get; set; }


        /// <summary>
        /// Application Cookie name used for user tracking
        /// </summary>
        public string ApplicationCookieName { get; set; }


        #region System Settings
        /// <summary>
        /// The database connection string for this WebLog instance
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// Determines how errors are displayed
        /// Default - ASP.NET Default behavior
        /// ApplicationErrorMessage - Application level error message
        /// DeveloperErrorMessage - StackTrace and full error info
        /// </summary>
        public DebugModes DebugMode { get; set; }

        #endregion



        #region Water Systems settings

        public string RawWater { get; set; }
        public string TreatedWater { get; set; }
        public string Raw1 { get; set; }
        public string Raw2 { get; set; }
        public string Raw3 { get; set; }
        public string Raw4 { get; set; }
        public string Treated1 { get; set; }
        public string Treated2 { get; set; }
        public string Treated3 { get; set; }
        public string Treated4 { get; set; }

        #endregion
    }

    /// <summary>
    /// Different modes that errors are displayed in the application
    /// </summary>
    public enum DebugModes
    {
        Default,
        ApplicationErrorMessage,
        DeveloperErrorMessage
    }
}