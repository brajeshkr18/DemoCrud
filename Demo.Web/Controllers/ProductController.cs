using Demo.Model.Customers;
using Demo.Model.Product;
using Demo.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Web.Controllers
{
    public class ProductController : Controller
    {
        IProductService objcustomer = new ProductService();
        // GET: Home  
        public ActionResult Index()
            {
                return View();
            }
            public JsonResult List()
            {
              
                return Json(objcustomer.GetAllProducts(), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Add(ProductViewModel emp)
            {
                return Json(objcustomer.SaveProducts(emp), JsonRequestBehavior.AllowGet);
            }
            public JsonResult GetbyID(int ID)
            {
                return Json(objcustomer.GetAllProducts().Find(x => x.Id.Equals(ID)), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Update(ProductViewModel emp)
            {
                return Json(objcustomer.UpdateProducts(emp), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Delete(int ID)
            {
                return Json(objcustomer.Delete(ID), JsonRequestBehavior.AllowGet);
            }
        
    }
}