using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Westwind.Utilities;

namespace GwcltdApp.Web.CustomConfiguration.ConfigModels
{
    public class custConfigType
    {
        public List<string> SimNumbers { get; set; }

        public static custConfigType FromString(string data)
        {
            return StringSerializer.Deserialize<custConfigType>(data, ",");
        }

        public override string ToString()
        {
            return StringSerializer.SerializeObject(this, ",");
        }
    }
}