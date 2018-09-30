using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Demo.Model.Customers
{
    public class CustomersViewModel
    {
           public int Id { get; set; }
            public int CustomerID { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }

            public string Address { get; set; }
        public System.DateTime ModifiedDate { get; set; }

        //public string Country { get; set; }


    }
}
