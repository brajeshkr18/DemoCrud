using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Service.Master;
using Demo.Web.Helper;
using Demo.Model.Master;
using Demo.Utility;
using System.Text.RegularExpressions;
using PagedList;
using Demo.Utility.Helper;
using Demo.Model.Customers;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using Demo.Web.Helper;

namespace Demo.Web.Controllers
{
    [HandleError]
    [CustomAuthorize]
    public class UserController : Controller
    {
        public static string Root = AppDomain.CurrentDomain.BaseDirectory;
        IUserService _IUserService = new UserService();
        ICustomerService _ICustomerService = new CustomerService();
        IMasterService _IMasterService = new MasterService();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Dashboard()
        //{                  //    return View();
        //}

        public ActionResult SendEmailNotificationForDrivers()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMailAllDriver(string message)
        {
            string mailBody = string.Empty;
            List<UserViewModel> driverList = new List<UserViewModel>();
            SearchingParams searchingParams = new SearchingParams();
            searchingParams.LoggedUserTypeCode = UserAuthenticate.Role;
            searchingParams.UserTypeCode = AppConstant.RoleDriver;
            //if (!string.IsNullOrWhiteSpace(userId))
            //    searchingParams.ReportingTo = Convert.ToInt64(userId);
            //else

            searchingParams.ReportingTo = Convert.ToInt64(UserAuthenticate.LogId);

            driverList = _IUserService.GetAllUsers(searchingParams);
            foreach (UserViewModel driver in driverList)
            {
                driver.Message = message;
                mailBody = RenderPartialToString("_NotifyToAllDriver", driver, ControllerContext);
                _IMasterService.SendAccountCreatationEmail("Driver Alert", mailBody, driver, Convert.ToInt64(UserAuthenticate.LogId) );
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [CustomActionFilter(ParamName = "data")]
        public ActionResult ManageUsers(string sortOrder, int? page, int? pageSize, string data, string search)
        {
            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.PageSize = pageDataSize;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "CreatedOn_desc" : "";

            Qparams qparams = null;
            if (!string.IsNullOrWhiteSpace(data))
                qparams = Helper.Helper.DecryptParamData<Qparams>(data);
            qparams.UserType = AppConstant.RoleDriver;
            TempData.Remove("qparams");
            TempData["qparams"] = qparams;
            TempData.Keep("qparams");

            SearchingParams searchingParams = Helper.Helper.GetParamData<SearchingParams>(search);

            if (qparams.IsProfile == true)
            {
                searchingParams.ReportingTo = Convert.ToInt64(qparams.ReportingTo);
            }
            else
            {
                searchingParams.ReportingTo = Convert.ToInt64(UserAuthenticate.LogId);
            }

            if (qparams.AccountStatus != null)
                searchingParams.AccountStatus = qparams.AccountStatus;

            searchingParams.UserTypeCode = string.Empty;
            searchingParams.LoggedUserTypeCode = UserAuthenticate.Role;

            if (!string.IsNullOrWhiteSpace(qparams.UserType))
            {
                if (!string.IsNullOrWhiteSpace(qparams.UserType))
                {
                    searchingParams.UserTypeCode = qparams.UserType.Trim();
                }
            }
            else
            {
                List<UserTypeViewModel> userTypes = GetUserTypes(qparams);
                ViewBag.UserTypes = userTypes;
                if (searchingParams.UserTypeId == null || searchingParams.UserTypeId?.Count == 0)
                    searchingParams.UserTypeId = userTypes.Select(item => item.Id).ToList();
            }


            var list = _IUserService.GetAllUsers(searchingParams).OrderBy(x => x.FullName).ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Users", list) : View(list);
        }


        [HttpGet]
        [CustomActionFilter(ParamName = "data")]
        public ActionResult UserDetails(string data)
        {
            float? CostPerMile = 0;
            float? CostPerMinute = 0;
            Qparams qparams = null;
            DateTime From = DateTime.MinValue, To = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(data))
                qparams = Helper.Helper.DecryptParamData<Qparams>(data);

            TempData.Remove("qparams");
            TempData["qparams"] = qparams;
            TempData.Keep("qparams");

            ViewBag.UserTypes = GetUserTypes(qparams);

            // GetDefaultData();

            if (qparams.Id != null)
            {
                UserViewModel userDetail = new UserViewModel();
                userDetail = _IUserService.GetUsersDetailsById((int)qparams.Id);
                userDetail.ConfirmEmail = userDetail.Email;
                userDetail.ConfirmPassword = userDetail.Password;
                userDetail.DriverHistory = _IUserService.GetVehiclesByDriverId((int)qparams.Id);
                if (userDetail.UserDetails != null)
                {
                    if (userDetail.UserDetails.CostPerMile != null)
                    {
                        CostPerMile = userDetail.UserDetails.CostPerMile;

                    }
                    if (userDetail.UserDetails.CostPerMinute != null)
                    {
                        CostPerMinute = userDetail.UserDetails.CostPerMinute;
                    }
                }
                
                userDetail.WeeklyPerformanceList = _IUserService.GetPerformaceListByDriverId((int)qparams.Id, CostPerMile, CostPerMinute, From, To);
                //GetAddressDefaultEditData(userDetail.UserDetail);
                return View(userDetail);
            }
            else
            {
                return View(new UserViewModel());
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetails([Bind(Exclude = "UserType")] UserViewModel user)
        {
            Qparams qparams = new Qparams();
            if (TempData["qparams"] != null)
            {
                qparams = TempData["qparams"] as Qparams;
                TempData.Keep("qparams");
            }

            TempData.Remove("result");
            var jsonData = SecurityHelper.Decrypt(System.Web.HttpContext.Current.Request.Cookies["ES"]["US"].ToString());
            Hashtable decryptedData = JsonConvert.DeserializeObject<Hashtable>(jsonData);
            qparams.UserType = decryptedData["UserType"].ToString();
            qparams.LogId = Convert.ToInt64(decryptedData["LogId"]);
            bool resultStatus = false;
            string mailBody = string.Empty;

            ViewBag.UserTypes = GetUserTypes(qparams);

            if (user.ReportingTo == null)
            {
                user.ReportingTo = Convert.ToInt64(UserAuthenticate.LogId);
                ModelState.Remove("ReportingTo");
            }

            if (user.Id == 0)
            {
                ModelState.Remove("Id");
            }
            else
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (_IUserService.IsUserExists(user.Email, user.Id.ToString()))
                ModelState.AddModelError("Email", "Email is already taken by someone.");


            if (ModelState.IsValid)
            {
                string responseMsg = string.Empty;

                MyJsonResult result;
                if (user.Id > 0)
                {
                    user.ModifiedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    resultStatus = _IUserService.UpdateUsers(user);
                    responseMsg = "The driver information has been updated.";
                }
                else
                {
                    user.CreatedBy = Convert.ToInt64(UserAuthenticate.LogId);

                    //mailBody = HomeController.RenderPartialToString("_accountActivationTemplate", user, ControllerContext);
                    resultStatus = _IUserService.SaveUsers(user, Convert.ToInt64(UserAuthenticate.LogId), true, mailBody);
                    if (resultStatus)
                    {
                        // sending mail to user
                        mailBody = RenderPartialToString("_AccountCreatationNotify", user, ControllerContext);
                        _IMasterService.SendAccountCreatationEmail("Driver Created", mailBody, user, Convert.ToInt64(UserAuthenticate.LogId));
                    }
                    responseMsg = "The driver account for " + user.FirstName + " " + user.LastName + " has been created.";
                }

                if (resultStatus)
                {
                    result = MyJsonResult.CreateSuccess(responseMsg);
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return RedirectToAction("ManageUsers", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(qparams)) });

                }
                else
                {
                    result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return View(user);
                }
            }
            else
            {
                //GetAddressDefaultEditData(user.UserDetail);
                return View(user);
            }
        }



        [RoleAuthorize(AppConstant.RoleAdmin)]
        public void Delete(int? Userid)
        {
            string[] names = { AppConstant.RoleAdmin };

            MyJsonResult result;
            try
            {
                int id = (int)Userid;
                int status = _IUserService.Delete(id);

                if (status==1)
                {
                    result = MyJsonResult.CreateSuccess("Driver has been deleted.");
                }
                else if (status == 2)
                {
                    result = MyJsonResult.CreateError("Cannot delete driver as there is  a trip assigned to for this driver ");
                }
                else 
                {
                    result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                }
                TempData["result"] = result;
                TempData.Keep("result");
                // return View();

            }
            catch (Exception ex)
            {
                result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                TempData["result"] = result;
                TempData.Keep("result");
                //return View();
            }


        }


        [AllowAnonymous]
        public JsonResult IsUserEmailExists(string Email, string Id)
        {
            return Json(!_IUserService.IsUserExists(Email, Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsers(string userId, string userTypeCode)
        {
            SearchingParams searchingParams = new SearchingParams();
            searchingParams.UserTypeCode = userTypeCode;
            searchingParams.LoggedUserTypeCode = UserAuthenticate.Role;
            if (!string.IsNullOrWhiteSpace(userId))
                searchingParams.ReportingTo = Convert.ToInt64(userId);
            else
                searchingParams.ReportingTo = Convert.ToInt64(UserAuthenticate.LogId);

            var list = _IUserService.GetUsersForDropDown(searchingParams);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUsersForDropDown(string search)
        {
            SearchingParams searchingParams = Helper.Helper.GetParamData<SearchingParams>(search);

            searchingParams.LoggedUserTypeCode = UserAuthenticate.Role;
            if (searchingParams.UserId != null)
                searchingParams.ReportingTo = Convert.ToInt64(searchingParams.UserId);
            else
                searchingParams.ReportingTo = Convert.ToInt64(UserAuthenticate.LogId);

            var list = _IUserService.GetUsersForDropDown(searchingParams);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserProfileInfo()
        {
            //Medium media = new Medium();
            UserViewModel userDetail = new UserViewModel();

            var Id = Convert.ToInt64(UserAuthenticate.LogId);
            userDetail = _IUserService.GetUsersDetailsById((int)Id);
            //media = _IUserService.GetUsersProfilePicById(userDetail.Id);
            //if (media != null)
            //{
            //    userDetail.ProfilePhoto = media.ImageBinary;
            //}

            return View(userDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfileInfo(UserViewModel user)
        {
            Qparams qparams = null;
            if (TempData["qparams"] != null)
            {
                qparams = TempData["qparams"] as Qparams;
                TempData.Keep("qparams");
            }

            TempData.Remove("result");
            bool resultStatus = false;
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmEmail");
            ModelState.Remove("Email");
            ModelState.Remove("ReportingTo");
            //Medium profilePhoto = new Medium();

            //HttpPostedFileBase imgFile = Request.Files["imageProfilePhoto"];
            //if (imgFile != null && imgFile.ContentLength > 0)
            //{
            //    if (user.Id > 0)
            //        profilePhoto.MediaForId = user.Id;

            //    profilePhoto.MediaFor = (int)Utility.Enums.MediaFor.UserProfile;
            //    profilePhoto.MediaType = (int)Utility.Enums.MediaType.Image;
            //    profilePhoto.ImageType = (int)Utility.Enums.ImageType.UserProfile;
            //    profilePhoto.MimeType = string.Empty;
            //    profilePhoto.ContentType = imgFile.ContentType;
            //    profilePhoto.FileName = imgFile.FileName;
            //    profilePhoto.ImageBinary = CommonHelper.ConvertToBytes(imgFile);
            //    profilePhoto.IsActive = true;
            //}

            if (ModelState.IsValid)
            {

                string responseMsg = string.Empty;
                MyJsonResult result;
                if (user.Id > 0)
                {
                    user.ModifiedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    resultStatus = _IUserService.UpdateUsers(user);
                    responseMsg = "The user information has been updated.";
                }

                if (resultStatus)
                {
                    result = MyJsonResult.CreateSuccess(responseMsg);
                    TempData["result"] = result;
                    TempData.Keep("result");


                    return RedirectToAction("UserProfileInfo");
                }
                else
                {
                    result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return View(user);
                }
            }
            return View(user);
        }

        [HttpPost]
        public void ChangeAccountStatus(long id, int accountStatus)
        {
            MyJsonResult result;
            string mailBody = string.Empty;
            string logId = UserAuthenticate.LogId;
            try
            {
                result = _IUserService.ChangeAccountStatus(id, accountStatus);
                if (result.isSuccess)
                {
                    UserViewModel userData = _IUserService.GetUsersDetailsById(id);
                    string subject = string.Empty;

                    if (accountStatus == (int)Enums.AccountStatus.Active)
                    {
                        subject = "Account has been activated!";
                        result.message = "Account has been activated successfully";

                        // sending mail to user account status
                        mailBody = RenderPartialToString("_Notify", userData, ControllerContext);
                        _IMasterService.SendAccountStatusEmail(subject, mailBody, userData, logId);

                    }
                    else if (accountStatus == (int)Enums.AccountStatus.Deactivated)
                    {
                        subject = "Account has been deactivated !";
                        result.message = "Account has been deactivated successfully";

                        // sending mail to user account status
                        mailBody = RenderPartialToString("_Notify", userData, ControllerContext);
                        _IMasterService.SendAccountStatusEmail(subject, mailBody, userData, logId);
                    }

                    else if (accountStatus == (int)Enums.AccountStatus.Suspended)
                    {
                        subject = "Account has been suspended !";
                        result.message = "Account has been suspended successfully";
                    }


                    //string mailBody = HomeController.RenderPartialToString("_ChangeAccountStatus", userData, ControllerContext);
                    //_IMasterService.SendEmail(Enums.MailType.AccountStatusChanged, userData, Convert.ToInt64(UserAuthenticate.LogId), subject, mailBody);
                }
                TempData["result"] = result;
                TempData.Keep("result");

            }
            catch (Exception ex)
            {
                result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                TempData["result"] = result;
                TempData.Keep("result");
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

        [HttpPost]
        public JsonResult ChangePassword(string userId, string password)
        {
            MyJsonResult result;
            try
            {
                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
                Match match = regex.Match(password);
                if (!match.Success)
                {
                    result = MyJsonResult.CreateError("Invalid Password");
                }

                string passwordHash = SecurityHelper.CreatePasswordHash(password, AppConfig.SaltKey);
                bool status = _IUserService.ChangePassword(userId, passwordHash);
                if (status)
                {
                    result = MyJsonResult.CreateSuccess("Password has been changed successfully.");
                }
                else
                {
                    result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [CustomActionFilter(ParamName = "data")]
        public ActionResult CustomerDetails(string data)
        {

            Qparams qparams = null;
            if (!string.IsNullOrWhiteSpace(data))
                qparams = Helper.Helper.DecryptParamData<Qparams>(data);

            TempData.Remove("qparams");
            TempData["qparams"] = qparams;
            TempData.Keep("qparams");

            // ViewBag.UserTypes = GetUserTypes(qparams);

            GetDefaultData();

            if (qparams != null)
            {
                if (qparams.Id != null)
                {
                    ViewBag.Title = "Update Information";

                    UserViewModel userDetail = new UserViewModel();
                    userDetail = _IUserService.GetUsersDetailsById((int)qparams.Id);
                    userDetail.ConfirmEmail = userDetail.Email;
                    userDetail.ConfirmPassword = userDetail.Password;
                    //GetAddressDefaultEditData(userDetail.UserDetail);
                    return View(userDetail);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ViewBag.Title = "Create new user";
                return View();

            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerDetails(CustomerViewModel customer)
        {

            Qparams qparams = null;
            if (TempData["qparams"] != null)
            {
                qparams = TempData["qparams"] as Qparams;
                TempData.Keep("qparams");
            }

            TempData.Remove("result");
            GetDefaultData();
            bool resultStatus = false;
            string mailBody = string.Empty;
            //ViewBag.UserTypes = GetUserTypes(qparams);

            //if (customer.ReportingTo == null)
            //{
            //    customer.ReportingTo = Convert.ToInt64(UserAuthenticate.LogId);
            //    ModelState.Remove("ReportingTo");
            //}

            //if (customer.Id == 0)
            //{
            //    ModelState.Remove("Id");
            //}
            //else
            //{
            //    ModelState.Remove("Password");
            //    ModelState.Remove("ConfirmPassword");
            //}

            //if (_IUserService.RegionAssigned(user.Id, user.AssignedRegionId, user.UserTypeId))
            //    ModelState.AddModelError("AssignedRegionId", "Regional Manager is already assigned to this region.");


            if (_IUserService.IsUserExists(customer.Email, customer.Id.ToString()))
                ModelState.AddModelError("Email", "Email is already taken by someone.");

            if (customer.Id == 0)
            {
                ModelState.Remove("Id");
            }
            if (ModelState.IsValid)
            {

                string responseMsg = string.Empty;
                // User IsAuthenticatedUser = new User();
                // IsAuthenticatedUser = _IUserService.UserAuthentication(user);

                MyJsonResult result;
                if (customer.Id > 0)
                {
                    // customer.ModifiedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    resultStatus = _ICustomerService.UpdateCustomers(customer);
                    responseMsg = "The Customer information has been updated.";

                }
                else
                {
                    //customer.CreatedBy = Convert.ToInt64(UserAuthenticate.LogId);
                    customer.PasswordHash = SecurityHelper.CreatePasswordHash(customer.PasswordHash, AppConfig.SaltKey);
                    //mailBody = HomeController.RenderPartialToString("_accountActivationTemplate", user, ControllerContext);
                    resultStatus = _ICustomerService.SaveCustomers(customer, Convert.ToInt64(UserAuthenticate.LogId), true, mailBody);

                    responseMsg = "The Customer account for " + customer.FirstName + " " + customer.LastName + " has been created.";
                }

                if (resultStatus)
                {
                    result = MyJsonResult.CreateSuccess(responseMsg);
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return RedirectToAction("ManageUsers", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(qparams)) });

                }
                else
                {
                    result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return View(customer);
                }
            }
            else
            {
                //GetAddressDefaultEditData(user.UserDetail);
                return View(customer);
            }
        }
     
        public ActionResult RenderPartialDriverPerformance(DateTime startDate,DateTime endDate,long DriverId)
        {
            UserViewModel userDetail = new UserViewModel();
            userDetail = _IUserService.GetUsersDetailsById((int)DriverId);
            float? CostPerMile = 0;
            float? CostPerMinute = 0;
            if (userDetail.UserDetails != null)
            {
                if (userDetail.UserDetails.CostPerMile != null)
                {
                    CostPerMile = userDetail.UserDetails.CostPerMile;

                }
                if (userDetail.UserDetails.CostPerMinute != null)
                {
                    CostPerMinute = userDetail.UserDetails.CostPerMinute;
                }
            }
            List<WeeklyPerformance> WeeklyPerformanceList= _IUserService.GetPerformaceListByDriverId((int)DriverId, CostPerMile, CostPerMinute, startDate, endDate);
            return PartialView("_DriverPerformanceList", WeeklyPerformanceList);
        }

        #region "Private Methods"
        private void GetDefaultData()
        {
            ViewBag.Countries = _IMasterService.GetCountryList();
        }

        private void GetAddressDefaultEditData(UserDetailViewModel usermembership)
        {
            ViewBag.States = _IMasterService.GetStateList(usermembership.CurCountry);
            ViewBag.Cities = _IMasterService.GetCityList(usermembership.CurState);
            //ViewBag.Regions = _IMasterService.GetRegionList(usermembership.CurCity);
        }


        private List<UserTypeViewModel> GetUserTypes(Qparams qparams)
        {
            List<UserTypeViewModel> userTypes = new List<UserTypeViewModel>();

            string[] roles = null;
            if (!string.IsNullOrWhiteSpace(qparams.UserType))
            {
                userTypes = _IUserService.GetUserTypes().Where(item => item.Code == qparams.UserType).ToList();
            }
            else
            {
                if (UserAuthenticate.Role == AppConstant.RoleAdmin)
                {
                    roles = new string[] { AppConstant.RoleEmployee, AppConstant.RoleDriver };
                    userTypes = _IUserService.GetUserTypes().Where(item => roles.Contains(item.Code)).ToList();
                }
                else if (UserAuthenticate.Role == AppConstant.RoleEmployee)
                {
                    roles = new string[] { AppConstant.RoleDriver };
                    userTypes = _IUserService.GetUserTypes().Where(item => roles.Contains(item.Code)).ToList();
                }
                else
                {
                    userTypes = _IUserService.GetUserTypes().Where(item => item.Code == qparams.UserType).ToList();
                }
            }




            return userTypes;
        }
        #endregion

    }
}