using System;
using System.Configuration;

namespace Demo.Model.Master
{
    /// <summary>
    /// Get the App Setting nodes
    /// </summary>
    public static class AppConfig
    {
        public static string FromEmail { get; set; }
        public static string FromPassword { get; set; }
        public static string FromUserName { get; set; }
        public static string Port { get; set; }
        public static string MailTo { get; set; }
        public static string Host { get; set; }
        public static bool isAutoInvite { get; set; }
        public static string SaltKey { get; set; }
        public static string TockenIdPreFix { get; set; }
        public static string ServiceInterval { get; set; }
        public static string Txn_IdPreFix { get; set; }
        public static string CurrencyCode { get; set; }
        public static string BatchSize { get; set; }
        public static bool UserLoginDeviceEnable { get; set; }
        public static int ReadyForRenewalDays { get; set; }
        public static int ReleasingMinutes { get; set; }
        public static int SendNotificationInterval { get; set; }
 

        public static string SiteURL { get; set; }
  
        public static string MaintenanceCode { get; set; }

        public static string InvoiceIdPreFix { get; set; }
        /// <summary>
        /// class constructor
        /// </summary>
        static AppConfig()
        {
            FromEmail = ConfigurationManager.AppSettings["fromEmail"]?.ToString();
            FromPassword = ConfigurationManager.AppSettings["fromPassword"]?.ToString();
            FromUserName = ConfigurationManager.AppSettings["fromUserName"]?.ToString();
            Port = ConfigurationManager.AppSettings["port"]?.ToString();
            MailTo = ConfigurationManager.AppSettings["mailTo"]?.ToString();
            Host = ConfigurationManager.AppSettings["host"]?.ToString();
            SaltKey = ConfigurationManager.AppSettings["saltKey"]?.ToString();          
            TockenIdPreFix = ConfigurationManager.AppSettings["TockenIdPreFix"]?.ToString();
            ServiceInterval = ConfigurationManager.AppSettings["ServiceInterval"]?.ToString();
            Txn_IdPreFix = ConfigurationManager.AppSettings["Txn_IdPreFix"]?.ToString();
            CurrencyCode = ConfigurationManager.AppSettings["CurrencyCode"]?.ToString();
            BatchSize = ConfigurationManager.AppSettings["batchSize"]?.ToString();
            ReadyForRenewalDays = Convert.ToInt32(ConfigurationManager.AppSettings["ReadyForRenewalDays"]?.ToString());
           
            ReleasingMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["releasingMinutes"]?.ToString());
            SendNotificationInterval = Convert.ToInt32(ConfigurationManager.AppSettings["SendNotificationInterval"]?.ToString());          
            SiteURL = ConfigurationManager.AppSettings["SiteURL"]?.ToString();           
            MaintenanceCode = ConfigurationManager.AppSettings["MaintenanceCode"]?.ToString();
            InvoiceIdPreFix = ConfigurationManager.AppSettings["InvoiceIdPreFix"]?.ToString();

            isAutoInvite = false;
            if (ConfigurationManager.AppSettings["autoInvite"]?.ToString() != null)
            {
                if (ConfigurationManager.AppSettings["autoInvite"].ToString().Equals("YES", StringComparison.InvariantCultureIgnoreCase))
                {
                    isAutoInvite = true;
                }

            }

            UserLoginDeviceEnable = false;
            if (ConfigurationManager.AppSettings["UserLoginDeviceEnable"]?.ToString() != null)
            {
                if (ConfigurationManager.AppSettings["UserLoginDeviceEnable"].ToString().Equals("YES", StringComparison.InvariantCultureIgnoreCase))
                {
                    UserLoginDeviceEnable = true;
                }

            }
        }
    }
}
