using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Demo.Utility.Helper;
using Demo.Model.Master;
using Demo.Web.Helper;
using Demo.Utility;

namespace Demo.Web.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {



      
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (UserAuthenticate.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Request.Cookies["ES"].Expires = DateTime.Now.AddHours(24);
                return RedirectToAction("Dashboard", "Dashboard", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new Qparams() { LogId = Convert.ToInt64(UserAuthenticate.LogId) })) });

            }
            //else if (UserAuthenticate.IsAuthenticated)
            //{            //    UserAuthenticate.Logout(System.Web.HttpContext.Current);
            //}

            if (TempData["result"] != null)
            {
                var myJsonResult = TempData["result"] as MyJsonResult;
                if (myJsonResult.data != null)
                    return View(new LoginViewModel { Email = myJsonResult.data.ToString() });
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.PasswordHash = SecurityHelper.CreatePasswordHash(model.Password, AppConfig.SaltKey);

                    UserViewModel authenticatedUser = null;
                    authenticatedUser = _userService.LoginAuthentication(model);

                    if (authenticatedUser != null)
                    {
                        string rememberme = (model.RememberMe) ? "true" : "false";
                        UserAuthenticate.AddLoginCookie(authenticatedUser.FirstName + " " + authenticatedUser.LastName, authenticatedUser.UserTypeCode, authenticatedUser.Id.ToString(),
                                     authenticatedUser.UserTypeName, rememberme);
                        return RedirectToAction("Dashboard", "Dashboard", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new Qparams() { LogId = Convert.ToInt64(authenticatedUser.Id) })) });

                    }
                    else
                    {
                        ModelState.AddModelError("", "User Not Authenticated ");
                        // ViewBag.ErrorMsg = "Please check your username and password! ";
                    }
                }
                catch (CustomException customException)
                {
                    ModelState.AddModelError("", customException.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }

            }
            return View(model);
        }
      
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            string[] userTypes = { AppConstant.RoleDriver};
            ViewBag.UserTypes = _userService.GetUserTypes().Where(item => userTypes.Contains(item.Code)).ToList();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            bool resultStatus = false;
            ModelState.Remove("Id");
            string mailBody = string.Empty;
            string[] userTypes = { AppConstant.RoleDriver };
            ViewBag.UserTypes = _userService.GetUserTypes().Where(item => userTypes.Contains(item.Code)).ToList();

            if (ModelState.IsValid)
            {

                string responseMsg = string.Empty;
                MyJsonResult result;

                UserViewModel user = GetUser(model);
                //mailBody = HomeController.RenderPartialToString("_accountActivationTemplate", user, ControllerContext);
                resultStatus = _userService.SaveUsers(user, 0, false, mailBody);
                responseMsg = "The user account for " + user.FirstName + " " + user.LastName + " has been created and email is sent for account activation.";

                if (resultStatus)
                {
                    result = MyJsonResult.CreateSuccess(responseMsg);
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return RedirectToAction("Login");

                }
                else
                {
                    result = MyJsonResult.CreateError(AppConstant.ErrorMessage);
                    TempData["result"] = result;
                    TempData.Keep("result");

                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [ActionName("Activation")]
        [AllowAnonymous]
        [Route("{controller}/{action}/{data}")]
        public ActionResult ChangePassword(string data)
        {
            ChangePasswordViewModel changePasswordViewModel = Helper.Helper.DecryptParamData<ChangePasswordViewModel>(data);
            changePasswordViewModel.OldPassword = SecurityHelper.Encrypt(changePasswordViewModel.OldPassword);

            UserViewModel authenticatedUser = _userService.GetUsersDetailsByEmail(changePasswordViewModel.Email);

            MyJsonResult result;
            if (authenticatedUser == null)
            {
                result = MyJsonResult.CreateError("Unauthenticated request");
                TempData["result"] = result;
                TempData.Keep("result");
                return RedirectToAction("Login");
            }
            else
            {
                if (authenticatedUser.DefaultPassword == false)
                {
                    if (authenticatedUser.AccountStatus == (int)Utility.Enums.AccountStatus.Active)
                    {
                        result = MyJsonResult.CreateError("Account is already active. Please login");
                    }
                    else if (authenticatedUser.AccountStatus == (int)Utility.Enums.AccountStatus.Deactivated)
                    {
                        result = MyJsonResult.CreateError("Account has been deactivated. Please contact to administrator");
                    }
                    else if (authenticatedUser.AccountStatus == (int)Utility.Enums.AccountStatus.Suspended)
                    {
                        result = MyJsonResult.CreateError("Account has been suspended. Please contact to administrator");
                    }
                    else
                    {
                        result = MyJsonResult.CreateError("Unauthenticated request");
                    }
                    TempData["result"] = result;
                    TempData.Keep("result");
                    return RedirectToAction("Login");
                }
            }

            TempData["email"] = changePasswordViewModel.Email;
            return View("ChangePassword", changePasswordViewModel);
        }


        // POST: /Manage/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {

                ViewBag.IsActive = "false";
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.OldPassword = SecurityHelper.Decrypt(model.OldPassword);
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError("", "Current password and new password cannot be same.");
                    return View(model);
                }
                UserViewModel userModel = _userService.ChangePassword(model);
                if (!string.IsNullOrWhiteSpace(userModel.Id.ToString()))
                {
                    UserAuthenticate.Logout(System.Web.HttpContext.Current);
                    UserAuthenticate.AddLoginCookie(userModel.FirstName + " " + userModel.LastName, userModel.UserTypeCode, userModel.Id.ToString(),
                                      userModel.UserTypeName, "false");
                    return RedirectToAction("Dashboard", "Dashboard", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new Qparams() { LogId = Convert.ToInt64(UserAuthenticate.LogId) })) });                    
                }
            }
            catch (CustomException customException)
            {
                ModelState.AddModelError("", customException.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Due to some technical problem this process cannot be completed. Please try after some time.");
            }
            return View(model);

        }

        
        public ActionResult LogOff()
        {
            TempData.Keep("result");

            //if (UserAuthenticate.IsAuthenticated)
            //    _userService.UserLogOff(UserAuthenticate.LogId);
            UserAuthenticate.Logout(System.Web.HttpContext.Current);
            return View();
        }

        public ActionResult Unauthorized()
        {
            UserAuthenticate.Logout(System.Web.HttpContext.Current);
            return View();
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        private UserViewModel GetUser(RegisterViewModel registerViewModel)
        {
            UserViewModel user = new UserViewModel();
            user.UserTypeId = registerViewModel.UserTypeId;
            user.FirstName = registerViewModel.FirstName;
            user.LastName = registerViewModel.LastName;
            user.Email = registerViewModel.Email;
            user.PhoneNumber = registerViewModel.PhoneNumber;
            user.PasswordHash = SecurityHelper.CreatePasswordHash(registerViewModel.Password, AppConfig.SaltKey);
            user.Password = registerViewModel.Password;
            user.CreatedOn = DateTime.Now;
            return user;
        }
        
    }
}