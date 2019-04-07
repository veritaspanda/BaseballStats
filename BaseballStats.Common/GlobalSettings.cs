using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;

namespace BaseballStats.Common
{
    public class GlobalSettings
    {
        //private static NameValueCollection appSetting = ConfigurationManager.AppSettings;

        public static string Base64Encode(string plainText)
        {
            if(plainText == null)
            {
                return null;
            }
            else
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if(base64EncodedData == null)
            {
                return null;
            }
            else
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
        }
    }
}