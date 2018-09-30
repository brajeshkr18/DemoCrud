using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.Master
{
    /// <summary>
    /// Qparams Model for Query String params
    /// </summary>
    public class Qparams
    {
        public long? LogId { get; set; }
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public string UserType { get; set; }

        public long? Createdby { get; set; }
        public long? ReportingTo { get; set; }
        public int? IsActive { get; set; }

        public bool? IsProfile { get; set; }
        public bool? IsViewOnly { get; set; }
        public int? AccountStatus { get; set; }

        public int? VerificationStatus { get; set; }


        public bool? AddPermission { get; set; }

        public bool? BackRedirection { get; set; }

        public int? Status { get; set; }
        public string Code { get; set; }
        public bool? IsViewOnlyReport { get; set; }
        public int BookingType { get; set; }
        public string CompanyName { get; set; }
        public bool IsExportReport { get; set; }
        public long? DriverId { get; set; }
        public long? VehicleId { get; set; }
        public string VINNumber { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
