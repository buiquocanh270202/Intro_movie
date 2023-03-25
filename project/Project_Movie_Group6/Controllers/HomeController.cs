using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Project_Movie_Group6.Helpper;
using Project_Movie_Group6.Models;


namespace Project_Movie_Group6.Controllers
{
    public class HomeController : Controller
    {
        private readonly CenimaDBContext _context;

        public HomeController(CenimaDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id, String searchString, int? Page)
        {
            

                List<Movie> Movie = _context.Movies.Include(r => r.Rates).ToList();
                List<Genre> Gender = _context.Genres.ToList();
                List<Rate> rate = _context.Rates.ToList();

                var PageNumber = Page == null || Page <= 0 ? 1 : Page.Value;
                var pageSize = 4;
                var IsCustomers = _context.Movies.AsNoTracking().Include(x => x.Genre).Where(a => a.IsActive ==true).OrderByDescending(x => x.MovieId);
                PagedList<Movie> models = new PagedList<Movie>(IsCustomers, PageNumber, pageSize);


                if (id > 0)
                {
                    IsCustomers = _context.Movies.AsNoTracking().Include(x => x.Genre).Where(g => g.GenreId == id).Where(a => a.IsActive == true).OrderByDescending(x => x.MovieId);
                    models = new PagedList<Movie>(IsCustomers, PageNumber, pageSize);
                    Movie = _context.Movies.Include(r => r.Rates).Where(g => g.GenreId == id).ToList();
                }


                if (!String.IsNullOrEmpty(searchString))
                {
                    string search = Unsign.utf8Convert1(searchString).ToLower();
                    IsCustomers = _context.Movies.AsNoTracking().Include(x => x.Genre).Where(pro => pro.Image.Contains(search)).Where(a => a.IsActive == true).OrderByDescending(x => x.MovieId);
                    models = new PagedList<Movie>(IsCustomers, PageNumber, pageSize);
                    Movie = _context.Movies.Where(pro => pro.Title.Contains(searchString)).ToList();
                }
                ViewData["movie"] = Movie;
                ViewBag.CurrentPage = PageNumber;
                ViewData["gender"] = Gender;
                ViewBag.Rates = rate;
                return View(models);
            
        }
    }
}