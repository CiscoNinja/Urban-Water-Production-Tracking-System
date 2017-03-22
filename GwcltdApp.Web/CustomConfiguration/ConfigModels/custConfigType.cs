using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Westwind.Utilities;

namespace GwcltdApp.Web.CustomConfiguration.ConfigModels
{
    //[TypeConverter(typeof(List<string>)), Serializable()]
    public class custConfigType
    {
        public string SimNumbers { get; set; }

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