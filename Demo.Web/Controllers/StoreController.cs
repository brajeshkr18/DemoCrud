using Demo.Model.Customers;
using Demo.Model.Store;
using Demo.Service.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.Web.Controllers
{
    public class StoreController : Controller
    {
        IStoreService objcustomer = new StoreService();
        // GET: Home  
        public ActionResult Index()
            {
                return View();
            }
            public JsonResult List()
            {
              
                return Json(objcustomer.GetAllStores(), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Add(StoreViewModel emp)
            {
                return Json(objcustomer.SaveStores(emp), JsonRequestBehavior.AllowGet);
            }
            public JsonResult GetbyID(int ID)
            {
                return Json(objcustomer.GetAllStores().Find(x => x.Id.Equals(ID)), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Update(StoreViewModel emp)
            {
                return Json(objcustomer.UpdateStores(emp), JsonRequestBehavior.AllowGet);
            }
            public JsonResult Delete(int ID)
            {
                return Json(objcustomer.Delete(ID), JsonRequestBehavior.AllowGet);
            }
        
    }
}