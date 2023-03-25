using Microsoft.AspNetCore.Mvc;
using Project_Movie_Group6.Helpper;
using Project_Movie_Group6.Models;

namespace Project_Movie_Group6.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                return View();
            } else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }
    }
}
