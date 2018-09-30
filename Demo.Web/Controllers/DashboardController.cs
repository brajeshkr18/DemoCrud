using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Demo.Model.Master;
using Demo.Service.Master;
using Demo.Utility;
using Demo.Web.Helper;

namespace Demo.Web.Controllers
{
    [HandleError]
    [CustomAuthorize]
    public class DashboardController : Controller
    {

        // GET: Dashboard
   
        IMasterService _IMasterService = new MasterService();
        public ActionResult Dashboard(string data)

        {
           var Role = UserAuthenticate.Role; ViewBag.TripRequest = 0; ViewBag.InvoiceRequest = 0; ViewBag.PayChequeRequest = 0; ViewBag.DriverTripSheetRequest=0;
           // ViewBag.TripRequest = _ITripRequestService.dashboardlist();
          //  ViewBag.TripRequest = _ITripRequestService.GetAllBookingRequestsCount((int)Enums.BookingRequestStatus.TripRequest);
            //ViewBag.InvoiceRequest = _ITripRequestService.GetAllBookingRequestsCount((int)Enums.BookingRequestStatus.InvoiceRequest);
            //ViewBag.PayChequeRequest = _ITripRequestService.GetAllBookingRequestsCount((int)Enums.BookingRequestStatus.PayChequeRequest);
            //ViewBag.DriverTripSheetRequest = _ITripRequestService.GetAllBookingRequestsCount((int)Enums.BookingRequestStatus.DriverTripSheetRequest);
            // ChechOdometerReading();
            return View();
        }
        //[HttpGet]
        //public JsonResult AjaxGetCall()
        //{
        //    var rep = _IReportService.GetReportByYear();
        //    return Json(rep, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Charts(string data)
        {
            return View();
        }
        public ActionResult TripTrack()
        {
            TimeZoneInfo curTimeZone = TimeZoneInfo.Local;
            var yourTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById(curTimeZone.Id));
           // var date = DateConversion.UTCDateAndTime(DateTime.UtcNow, (curTimeZone.Id).ToString(), false);

            var yourTime1 = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            var yourTime2 = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById(curTimeZone.Id));
            ViewBag.Time1 = "";
            ViewBag.yourTime1 = yourTime1;
            ViewBag.yourTime2 = yourTime2;
            return View();
        }
      

        public ActionResult RoleAccess(string data)
        {
            return View();
        }
        [HttpPost]
        public ActionResult RoleAccess(string Text, bool IsEmail, bool IsSMS)
        {
            try
            {

                return View();
            
            }
            catch (Exception ex)
            {
                return View();
            }
        }
       
        public static string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");


            viewName = "~/Views/Shared/Email Templates/" + viewName + ".cshtml";
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}