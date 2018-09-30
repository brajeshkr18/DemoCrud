using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.Master
{
    public class MessageViewModel
    {
        public long Id { get; set; }
        public int RecieverId { get; set; }
        public int SenderId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> ActionById { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string FromNo { get; set; }
        public string ToNo { get; set; }
        public string CreatedByUser { get; set; }
        public string SendFrom  { get; set; }
        public string SendTo { get; set; }
        public string Direction { get; set; }
        public string AccountSid { get; set; }
        public string ApiVersion { get; set; }
        public string Sid { get; set; }
        public string Uri { get; set; }
        public string MessagingServiceSid { get; set; }
        public Nullable<System.DateTime> DateSent { get; set; }
    }
   
}

