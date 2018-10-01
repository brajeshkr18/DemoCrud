using Demo.Model.Sales;
using Demo.Service.Customer;
using Demo.Service.Product;
using Demo.Service.Sales;
using Demo.Service.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Web.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        IProductService _objProduct = new ProductService();
        ICustomerService _objcustomer = new CustomerService();
        IStoreService _objStore = new StoreService();
        ISalesService _objSales = new SalesService();
        public ActionResult Index()
        {
            List<SalesViewModel> lst = new List<SalesViewModel>();
            //SalesViewModel objSales = new SalesViewModel();
             lst = _objSales.GetAllSalesRecord(0);
            ViewBag.Product = new SelectList(_objProduct.GetProductsForDropDown(), "Id", "Name", 0);
            //ViewBag.Product = _objProduct.GetProductsForDropDown();
            ViewBag.Customers = _objcustomer.GetCustomersForDropDown();
            ViewBag.Stores = _objStore.GetStoresForDropDown();
            return View(lst);
        }
        public JsonResult Add(SalesViewModel objsales)
        {
            return Json(_objSales.SaveSales(objsales), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            return Json(_objSales.GetAllSalesRecord(ID), JsonRequestBehavior.AllowGet);
        }
    }
}