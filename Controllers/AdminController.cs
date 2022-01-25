using System.Web.Mvc;

namespace Dictionary.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
