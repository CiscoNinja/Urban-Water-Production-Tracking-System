using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Westwind.Utilities.Configuration;

namespace GwcltdApp.Web.AppConfig
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
                
                PropertiesToEncrypt = "MailServerPassword,ConnectionString",
                EncryptionKey = "secret",
                ConnectionString = "DevSampleConnectionString",
                Tablename = "Configuration",
                Key = 1
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

        /// <summary>
        /// Determines how many items are displayed per page in typical list displays
        /// </summary>
        public int MaxPageItems { get; set; }

        #endregion

       

        #region Water Systems settings

        public List<string> RawWaterList { get; set; }
        public List<string> TreatedWaterList { get; set; }
        public List<string> RawOne { get; set; }
        public List<string> RawTwo { get; set; }
        public List<string> RawThree { get; set; }
        public List<string> RawFour { get; set; }
        public List<string> TreatedOne { get; set; }
        public List<string> TreatedTwo { get; set; }
        public List<string> TreatedThree { get; set; }
        public List<string> TreatedFour { get; set; }

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