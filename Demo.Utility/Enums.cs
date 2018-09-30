using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Utility
{
    public class Enums
    {
        public enum AccountStatus
        {
            Active = 1,
            Inactive = 2,
            Suspended = 3,
            Deactivated = 4,
            Delete = 5
        }
        public enum Gender
        {
            Male = 1,
            Female = 2,
            Transgender = 3
        }
        public enum IsActive
        {
            Active = 1,
            Inactive = 0
        }

        public enum NotificationStatus
        {
            Initiated = 1,
            Sent = 2,
            Failed = 3

        }
        public enum MailType
        {
            AccountCreation = 1,
            AccountActivation = 2,
            SendAllDriver = 3
        }
        public enum PaymentProcessLevel
        {
            NA = 0,
            ReadyToInitiate = 1,
            Initiated = 2,
            Completed = 3
        }

        public enum ImageType
        {
            Logo = 1,
            Banner = 2,
            UserProfile = 3,
            Cheque = 4
        }

        public enum MediaFor
        {
            UserProfile = 1,
            Cheque = 2
        }
       
    }
}
