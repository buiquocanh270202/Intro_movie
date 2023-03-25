using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Project_Movie_Group6.Helpper;
using Project_Movie_Group6.Models;

namespace Project_Movie_Group6.Controllers
{
    public class MovieController : Controller
    {

        private readonly CenimaDBContext _context;

        public MovieController(CenimaDBContext context)
        {
            _context = context;
        }
        // GET: MovieController
        public ActionResult Index(int id)
        {
            if (id > 0)
            {
                var movies = _context.Movies.Include(g => g.Genre).Where(m => m.MovieId == id).FirstOrDefault();
                ViewData["movie"] = movies;
                var rates = _context.Rates.Include(p => p.Person).Where(m => m.MovieId == id).OrderByDescending(r => r.Time).ToList();
                
                var account = _context.Persons.ToList();
                var avg = rates.Average(a => a.NumericRating);
                ViewData["avg"] = avg;
                ViewData["rate"] = rates;

            }
            return View();
        }

        public ActionResult CreateRate(int id)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("account") != null)
            {
                if (id > 0)
                {
                    int check = id;
                    var movies = _context.Movies.Include(g => g.Genre).Where(m => m.MovieId == id).FirstOrDefault();
                    ViewData["movie"] = movies;
                    var rates1 = _context.Rates.ToList();

                    var rates = _context.Rates.Include(p => p.Person).Where(m => m.MovieId == id).OrderByDescending(r => r.Time).ToList();
                    LoginModel p = HttpContext.Session.GetObjectFromJson<LoginModel>("account");

                    Person person = _context.Persons.FirstOrDefault(x => x.Email == p.Email);

                    //(LoginModel)JsonSerializer.Deserialize<LoginModel>(Context.Session.GetString("admin"))


                    var avg = rates.Average(a => a.NumericRating);
                    ViewData["rateCreateRate"] = rates1;
                    ViewData["rates"] = rates;
                    ViewData["avg"] = avg;
                    ViewData["person"] = person;

                }

                return View();
            } else
            {
                return RedirectToAction("Index", "Home");

            }
        }


        [HttpPost]
        public IActionResult Update(Rate rate)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("account") != null)
            {
                if (rate.Comment != null && rate.NumericRating != null)
                {
                    var r = _context.Rates.Where(p => p.PersonId == rate.PersonId && p.MovieId == rate.MovieId).FirstOrDefault();
                    if (r != null)
                    {
                        r.NumericRating = rate.NumericRating;
                        r.Time = DateTime.Now;
                        r.Comment = rate.Comment;
                        _context.Update(r);
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public IActionResult Add(Rate rate)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("account") != null)
            {
                if (rate.Comment != null && rate.NumericRating != null)
                {
                    rate.Time = DateTime.Now;
                    _context.Add(rate);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            } else
            {
                return RedirectToAction("Index", "Home");
            }
        } 
        



    }
}
