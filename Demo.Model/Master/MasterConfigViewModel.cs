using System;

namespace Demo.Model.Master
{
    public class MasterConfigViewModel
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string ConfigXML { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public System.DateTime CreatedOn { get; set; }
    }
}
