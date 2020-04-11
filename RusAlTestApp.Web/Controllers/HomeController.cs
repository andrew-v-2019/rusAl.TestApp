using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RusAlTestApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public IActionResult AdmIndex()
        {
            return View();
        }
    }
}
