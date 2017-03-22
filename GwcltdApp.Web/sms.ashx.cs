using GwcltdApp.Web.CustomConfiguration;
using GwcltdApp.Web.DAL;
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

            #region retrieve config values
            string option = null;
            int optionid = 0;
            string optiontype = null;
            int optiontypeid = 0;
            string[] rawWater = Regex.Split(App.Configuration.RawWater, "\r\n");
            string[] treatedWater = Regex.Split(App.Configuration.TreatedWater, "\r\n");
            string[] raw1 = Regex.Split(App.Configuration.Raw1, "\r\n");
            string[] raw2 = Regex.Split(App.Configuration.Raw2, "\r\n");
            string[] raw3 = Regex.Split(App.Configuration.Raw3, "\r\n");
            string[] raw4 = Regex.Split(App.Configuration.Raw4, "\r\n");
            string[] treated1 = Regex.Split(App.Configuration.Treated1, "\r\n");
            string[] treated2 = Regex.Split(App.Configuration.Treated2, "\r\n");
            string[] treated3 = Regex.Split(App.Configuration.Treated3, "\r\n");
            string[] treated4 = Regex.Split(App.Configuration.Treated4, "\r\n");

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
            #endregion retrieve config values

            // Text Message processing
            // Split the string on line breaks.
            // ... The return value from Split is a string array.

            #region processing text message into database
            string[] lines = Regex.Split(messageText, "\r\n");

            foreach (string line in lines)
            {
                DateTime DateCreated;
                DateTime DayToRecord;
                //double DailyActual;
                double FRPH;
                double FRPS;
                double TFPD;
                double NTFPD;
                double LOG;
                //string WSystem;
                //string WSystemCode;
                //int WSystemId;
                string Option;
                int OptionId;
                string OptionType;
                int OptionTypeId;
                string Comments;

                //string value = "YAC0415 06:00 08/11/2016 39C 194.108 m3/h v:0.76 m/s 640794.4 m3 -17.3992 m3 s9 LOG: 96";
                // Split the string on line breaks.
                // ... The return value from Split is a string array.
                //string[] lines = value.Split(' ');

                DateCreated = Convert.ToDateTime(lines[2] + " " + lines[1]);
                DayToRecord = Convert.ToDateTime(lines[2] + " " + lines[1]);
                TFPD = Convert.ToDouble(lines[8]);
                NTFPD = Convert.ToDouble(lines[10]);
                LOG = Convert.ToDouble(lines[14]);
                FRPH = Convert.ToDouble(lines[4]);
                FRPS = Convert.ToDouble(lines[6].Remove(0, 2));
                Option = option;
                OptionId = optionid;
                OptionType = optiontype;
                OptionTypeId = optiontypeid;
                Comments = "Send Via Text Message";
                //DailyActual = Convert.ToDouble(lines[17]);

                //foreach (string line in lines)
                //{
                //    Console.WriteLine(line);
                //}
                Console.WriteLine(DayToRecord);
                Console.WriteLine(DateCreated);
                Console.WriteLine(TFPD);
                Console.WriteLine(NTFPD);
                Console.WriteLine(LOG);
                Console.WriteLine(FRPS);
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