using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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

            /*
             * TODO: IMPLEMENT ANY PROCESSING HERE THAT YOU NEED TO PERFORM UPON RECEIPT OF A MESSAGE
             *
             */


            // Sending a reply SMS. If you don't want to send a reply, just comment all the next lines out
            context.Response.ContentType = "text/plain";

            // By default the reply is sent back to the sender of the original sms message
            // You can change the recipient(s) by adding an X-SMS-To header to the HTTP response.
            // You can specify multiple numbers by separating them with spaces. 
            // For example:      
            // context.Response.AddHeader("X-SMS-To", "+97771234567 +15550987654");

            // Write the text of the reply SMS message to the HTTP response output stream
            context.Response.Write("GWCLtd : Message Received ");
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