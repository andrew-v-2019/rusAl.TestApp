using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RusAlTestApp.Data;
using RusAlTestApp.Web.Models;

namespace RusAlTestApp.Web.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationContext _context;
        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
