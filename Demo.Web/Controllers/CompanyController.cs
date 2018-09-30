using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Model.Master;
using Demo.Model.Users;
using Demo.Model.Vehicle;
using Demo.Service.Master;
using Demo.Service.User;
using Demo.Service.Vehicle;
using Demo.Utility;
using Demo.Utility.Helper;
using Demo.Web.Helper;

namespace Demo.Web.Controllers
{
    [HandleError]
    [CustomAuthorize]
    public class CompanyController : Controller
    {
        // GET: Company
        IUserService _IUserService = new UserService();
        IMasterService _IMasterService = new MasterService();
        IVehicleService _IVehicleService = new VehicleService();
        [HttpGet]
        [CustomActionFilter(ParamName = "data")]
        public ActionResult CompanyDetails(string data)
        {
            CompanyViewModel comapnyDetail = new CompanyViewModel();
            Qparams qparams = null;
            if (!string.IsNullOrWhiteSpace(data))
                qparams = Helper.Helper.DecryptParamData<Qparams>(data);

            TempData.Remove("qparams");
            TempData["qparams"] = qparams;
            TempData.Keep("qparams");
            var values = from Enums.BookingType e in Enum.GetValues(typeof(Enums.BookingType))
                         select new
                         {
                             ID = (int)Enum.Parse(typeof(Enums.BookingType), e.ToString()),
                             Name = e.ToString()
                         };
            ViewBag.CompanyList = new SelectList(values, "Id", "Name");
            ViewBag.StateList = _IMasterService.GetStateList(1);
            ViewBag.RailRoads = _IMasterService.GetRailRoads();
            if (qparams.Id != null)
            {

                comapnyDetail = _IMasterService.GetCompanyById((int)qparams.Id).FirstOrDefault();
                ViewBag.RailRoads = _IMasterService.GetRailRoadsByCompany((long)qparams.CompanyId);
                return View(comapnyDetail);
            }
            else
            {
               
                return View(comapnyDetail);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDetails(CompanyViewModel company)
        {

            Qparams qparams = null;
            if (TempData["qparams"] != null)
            {
                qparams = TempData["qparams"] as Qparams;
                TempData.Keep("qparams");
            }
            string responseMsg = string.Empty;
            MyJsonResult result;
            TempData.Remove("result");
            bool resultStatus = false;
          
            string mailBody = string.Empty;
            
            if (company.Id == 0)
            {
                ModelState.Remove("Id");
            }
            
            if (ModelState.IsValid)
            {
                if (company.Name == Enums.BookingType.Halcon.ToString())
                {
                    company.CompanyId = (int)Enums.BookingType.Halcon;
                }
               else if (company.Name == Enums.BookingType.PTI.ToString())
                {
                    company.CompanyId = (int)Enums.BookingType.PTI;
                }
               else if (company.Name == Enums.BookingType.CP.ToString())
                {
                    company.CompanyId = (int)Enums.BookingType.CP;
                }
                else
                {
                    company.CompanyId = (int)Enums.BookingType.BP;
                }
                if (company.Id > 0)
                {
                    
                    company.ModifiedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    company.CreatedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    resultStatus = _IMasterService.UpdateCompany(company);
                    responseMsg = "The company information has been updated.";
                }
                else
                {
                    company.CreatedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    resultStatus = _IMasterService.SaveCompany(company);
                    responseMsg = "The company has been created.";
                }
                result = MyJsonResult.CreateSuccess(responseMsg);
                TempData["result"] = result;
                return RedirectToAction("ManageCompany", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(qparams)) });
            }
            else
            {
               
                result = MyJsonResult.CreateError(responseMsg);
                TempData["result"] = result;
                TempData.Keep("result");
                return View(company);
            }
        }
        [CustomActionFilter(ParamName = "data")]
        public ActionResult ManageCompany(string sortOrder, int? page, int? pageSize, string data, string search)
        {
            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.PageSize = pageDataSize;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "CreatedOn_desc" : "";

            Qparams qparams = null;
            if (!string.IsNullOrWhiteSpace(data))
                qparams = Helper.Helper.DecryptParamData<Qparams>(data);

            TempData.Remove("qparams");
            TempData["qparams"] = qparams;
            TempData.Keep("qparams");

            SearchingParams searchingParams = Helper.Helper.GetParamData<SearchingParams>(search);

            var list = _IMasterService.GetCompanyById(0).OrderByDescending(item => item.Id).ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Companies", list) : View(list);
        }

        public ActionResult RenderPartialHistory()
        {
            CompanyViewModel company = new CompanyViewModel();
            company.CompanyHistoryList = _IMasterService.GetCompanyHistory();
            return PartialView("_CompanyHistory", company.CompanyHistoryList);
        }
        public JsonResult GetRailRoadsByCompany(int CompanyId)
        {
            return Json(_IMasterService.GetRailRoadsByCompany(CompanyId), JsonRequestBehavior.AllowGet);
        }

    }
}