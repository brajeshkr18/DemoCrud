using System.Web.Mvc;
using Demo.Service.Master;
//using Demo.Web.Helper;

namespace Demo.Web.Controllers
{
    //[HandleError]
    //[CustomAuthorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetImage(long id, int imageType, int mediaFor)
        {
            byte[] imageBytes = null;

            if (imageBytes == null)
            {
                //if (imageType == (int)Utility.Enums.ImageType.UserProfile)
                //    return new FilePathResult("~/images/user.png", "image/png");
                //else
                    return new FilePathResult("~/images/noimage.jpg", "image/png");
            }
            else
            {
                return new FileContentResult(imageBytes, "image/jpeg");
            }
        }
        public ActionResult TripTrack()
        {
            return View();
        }
    }
}