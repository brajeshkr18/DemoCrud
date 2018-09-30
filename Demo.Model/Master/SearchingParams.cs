using System;
using System.Collections.Generic;

namespace Demo.Model.Master
{
    /// <summary>
    /// SearchingParams Model for all searching params
    /// </summary>
    public class SearchingParams
    {
        public string Email { get; set; }
        public int? AccountStatus { get; set; }
        public int? Status { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public string Contact { get; set; }

        //public long? UserType { set; get; }
        public List<long> UserTypeId { set; get; }
        public string UserTypeCode { set; get; }
        public string LoggedUserTypeCode { set; get; }

        public int? Country { get; set; }

        public int? State { get; set; }
        public int? City { get; set; }
        public string ZipCode { get; set; }

        public long? UserId { get; set; }   
      
        public long? Createdby { get; set; }
        public long? ReportingTo { get; set; }

        public int Flag { set; get; }

        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public long? RequestId { get; set; }

        public long LogId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string VINNumber { get; set; }

        public string MaintenanceCode { get; set; }
        public string TrainNumber { get; set; }
        public Nullable<DateTime> PickUpdate { get; set; }
        public string CompanyName { get; set; }
        public string RRNumber { get; set; }
        public int? VehicleNo { get; set; }
        public Nullable<DateTime> InvoiceCreated { get; set; }
        public string InvoiceNumber { get; set; }

    }
}
