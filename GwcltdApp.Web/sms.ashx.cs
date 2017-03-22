using GwcltdApp.Entities;
using GwcltdApp.Web.CustomConfiguration;
using GwcltdApp.Web.DAL;
using GwcltdApp.Web.Infrastructure.Extensions;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GwcltdApp.Web
{
    /// <summary>
    /// Summary description for sms
    /// </summary>
    public class sms : IHttpHandler
    {
        // This method is called each time a new SMS message arrives
        public void ProcessRequest(HttpContext context)
        {
            #region text message fields
            // extracting sms message information from form variables

            NameValueCollection sms = context.Request.Form;

            // sender's number
            string senderNumber = sms["sender"];

            // message text
            string messageText = sms["text"];

            // SMS center timestamp in the local time zone of the computer on which SMS Enabler is running.       
            // You can consider this as the date and time when the sender sent the message      
            DateTime sentTime = DateTime.ParseExact(sms["sc_datetime"], "yyyy'-'MM'-'dd HH':'mm':'ss", null);
            // SMS center timestamp in UTC
            DateTime sentTimeUTC = DateTime.ParseExact(sms["sc_datetime_utc"], "yyyy'-'MM'-'dd HH':'mm':'ss", null);

            // SMS center number (not supported when SMS Enabler is used with a Nokia phone)
            string smscNumber = sms["smsc"];

            // Tag value. You can define this in SMS Enabler's settings, and use it to pass additional information.
            string tag = sms["tag"];
            #endregion text message fields
            /*
             * TODO: IMPLEMENT ANY PROCESSING HERE THAT YOU NEED TO PERFORM UPON RECEIPT OF A MESSAGE
             *
             */

            #region retrieving and mapping config values
            string option = null;
            int optionid = 0;
            string optiontype = null;
            int optiontypeid = 0;
            string[] rawWater = App.Configuration.RawWater.Split(',');
            string[] treatedWater = App.Configuration.TreatedWater.Split(',');
            string[] raw1 = App.Configuration.Raw1.Split(',');
            string[] raw2 = App.Configuration.Raw2.Split(',');
            string[] raw3 = App.Configuration.Raw3.Split(',');
            string[] raw4 = App.Configuration.Raw4.Split(',');
            string[] treated1 = App.Configuration.Treated1.Split(',');
            string[] treated2 = App.Configuration.Treated2.Split(',');
            string[] treated3 = App.Configuration.Treated3.Split(',');
            string[] treated4 = App.Configuration.Treated4.Split(',');

            if (rawWater.Contains(senderNumber))
            {
                option = "Raw Water";
                optionid = SummaryManager.GetTypeIdByName(option);
            }
            else if (treatedWater.Contains(senderNumber)) {
                option = "Treated Water";
                optionid = SummaryManager.GetTypeIdByName(option);
            }

            if (raw1.Contains(senderNumber))
            {
                optiontype = "Raw Water 1";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (raw2.Contains(senderNumber))
            {
                optiontype = "Raw Water 2";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (raw3.Contains(senderNumber))
            {
                optiontype = "Raw Water 3";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (raw4.Contains(senderNumber))
            {
                optiontype = "Raw Water 4";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (treated1.Contains(senderNumber))
            {
                optiontype = "Treated Water 1";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (treated2.Contains(senderNumber))
            {
                optiontype = "Treated Water 2";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (treated3.Contains(senderNumber))
            {
                optiontype = "Treated Water 3";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            else if (treated4.Contains(senderNumber))
            {
                optiontype = "Treated Water 4";
                optiontypeid = SummaryManager.GetTypeIdByName2(optiontype);
            }
            #endregion retrieving and mapping config values

            // Text Message processing
            // Split the string on line breaks.
            // ... The return value from Split is a string array.

            #region processing text message into database
            

            IEnumerable<string> allSystemNumbers = rawWater.Union(treatedWater);

            if (allSystemNumbers.Contains(senderNumber)) //ensures that only text messages coming from GWCLtd. Systems are processed
            {
                var productionVM = new ProductionViewModel();
                string[] lines = messageText.Split(' ');
                //Regex.Split(messageText, "\r\n");

                foreach (string line in lines)
                {
                    //example
                    //string value = "YAC0415 06:00 08/11/2016 39C 194.108 m3/h v:0.76 m/s 640794.4 m3 -17.3992 m3 s9 LOG: 96";
                    // Split the string on line breaks.
                    // ... The return value from Split is a string array.
                    //string[] lines = value.Split(' ');

                    productionVM.DateCreated = Convert.ToDateTime(lines[2] + " " + lines[1]);
                    productionVM.DayToRecord = Convert.ToDateTime(lines[2] + " " + lines[1]);
                    productionVM.DailyActual = ;
                    productionVM.Comment = "Sent Via Text Message from " + senderNumber;
                    productionVM.FRPH = Convert.ToDouble(lines[4]);
                    productionVM.FRPS = Convert.ToDouble(lines[6].Remove(0, 2));
                    productionVM.TFPD = Convert.ToDouble(lines[8]);
                    productionVM.NTFPD = Convert.ToDouble(lines[10]);
                    productionVM.LOG = Convert.ToDouble(lines[14]);
                    productionVM.WSystem = SummaryManager.GetSystemName(lines[0]);
                    productionVM.WSystemCode = lines[0];
                    productionVM.WSystemId = SummaryManager.GetSystemId(lines[0]);
                    productionVM.Option = option;
                    productionVM.OptionId = optionid;
                    productionVM.OptionType = optiontype;
                    productionVM.OptionTypeId = optiontypeid;
                    productionVM.GwclStationId = SummaryManager.GetStationId(lines[0]);
                    productionVM.StationCode = SummaryManager.GetStationCode(lines[0]);
                    productionVM.GwclStation = SummaryManager.GetSystemName(lines[0]);

                    Production newProduction = new Production();
                    newProduction.UpdateProduction(productionVM);
                }

            }

            #endregion processing text message into database 
            // Sending a reply SMS. If you don't want to send a reply, just comment all the next lines out

            #region reply to message
            context.Response.ContentType = "text/plain";

            // By default the reply is sent back to the sender of the original sms message
            // You can change the recipient(s) by adding an X-SMS-To header to the HTTP response.
            // You can specify multiple numbers by separating them with spaces. 
            // For example:      
            // context.Response.AddHeader("X-SMS-To", "+97771234567 +15550987654");

            // Write the text of the reply SMS message to the HTTP response output stream
            context.Response.Write("GWCLtd : Message Received ");
            #endregion reply to message
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}