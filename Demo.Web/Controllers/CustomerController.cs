using Demo.Model.Customers;
using Demo.Service.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Demo.Web.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerService objcustomer = new CustomerService();
        // GET: Home  
            public ActionResult Index()
            {
                return View();
            }
            public JsonResult List()
            {
              
                return Json(objcustomer.GetAllCustomer(), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Add(CustomersViewModel emp)
            {
                return Json(objcustomer.SaveCustomers(emp), JsonRequestBehavior.AllowGet);
            }
            public JsonResult GetbyID(int ID)
            {
                return Json(objcustomer.GetAllCustomer().Find(x => x.Id.Equals(ID)), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Update(CustomersViewModel emp)
            {
                if (objcustomer.UpdateCustomers(emp))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }
              
            }
            public JsonResult Delete(int ID)
            {
                return Json(objcustomer.Delete(ID), JsonRequestBehavior.AllowGet);
            }
        
    }
}