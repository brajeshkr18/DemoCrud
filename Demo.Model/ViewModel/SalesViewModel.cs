using Demo.Model.Customers;
using Demo.Model.Product;
using Demo.Model.Store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Demo.Model.Sales
{
    public class SalesViewModel
    {
        public long Id { get; set; }
        public System.DateTime PurchasedDate { get; set; }
        public  long ProductId { get; set; }
        public string ProductName { get; set; }
        public  long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public  long StoreId { get; set; }
        public string StoreName { get; set; }


    }
}
