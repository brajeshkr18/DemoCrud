using System;
using System.Web.Mvc;
using Demo.Web.Helper;
using Demo.Model.Master;

namespace Demo.Web.Controllers
{
    [HandleError]
    [CustomAuthorize]
    public class ManageController : Controller
    {
        IUserService _IUserService = new UserService();

        [HttpGet]
        [CustomAuthorize]
        public ActionResult ChangePassword()
        {
            ViewBag.Email = _IUserService.GetUsersDetailsById(Convert.ToInt64(UserAuthenticate.LogId)).Email;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            MyJsonResult result;
            try
            {

                ViewBag.IsActive = "false";
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model.OldPassword == model.NewPassword)
                {
                    result = MyJsonResult.CreateError("Current password and new password cannot be same");
                    TempData["result"] = result;
                    TempData.Keep("result");
                }
                else
                {
                    UserViewModel userModel = _IUserService.ChangePassword(model);
                    result = MyJsonResult.CreateSuccess("Password has been changed successfully.");
                    result.data = model.Email;
                    TempData["result"] = result;
                    TempData.Keep("result");

                    var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                    context.Clients.All.logoff(userModel.Id);

                    return RedirectToAction("LogOff", "Account");

                    //return RedirectToAction("Dashboard", "Dashboard", new { data = SecurityHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(new Qparams() { LogId = Convert.ToInt64(UserAuthenticate.LogId) })) });
                }
            }
            catch (CustomException customException)
            {
                result = MyJsonResult.CreateError(customException.Message);
                TempData["result"] = result;
                TempData.Keep("result");
            }
            catch (Exception ex)
            {
                result = MyJsonResult.CreateError("Due to some technical problem this process cannot be completed. Please try after some time.");
                TempData["result"] = result;
                TempData.Keep("result");
            }
            return View(model);

        }
    }
}