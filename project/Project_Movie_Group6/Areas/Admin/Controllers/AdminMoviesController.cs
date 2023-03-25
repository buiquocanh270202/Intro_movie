using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Movie_Group6.Helpper;
using Project_Movie_Group6.Models;

namespace Project_Movie_Group6.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMoviesController : Controller
    {
        private readonly CenimaDBContext _context;

        public AdminMoviesController(CenimaDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Movie Movie { get; set; } = default!;

        // GET: Admin/AdminMovies
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                var cenimaDBContext = _context.Movies.Include(m => m.Genre).ToList();
                return View(cenimaDBContext);
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }

        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                if (id == null || _context.Movies == null)
                {
                    return NotFound();
                }

                var movie = _context.Movies
                    .Include(m => m.Genre)
                    .FirstOrDefaultAsync(m => m.MovieId == id);


                return View(movie);
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Description");
                return View();
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }

        
        [HttpPost]
        
        public async Task<IActionResult> CreateAsync(Movie movie, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (fThumb != null)
            {
                string extension = Path.GetExtension(fThumb.FileName);
                string imageName = Utilities.SEOUrl(movie.Title) + extension;
                movie.Image = await Utilities.UploadFile(fThumb, @"MoviesImg", imageName.ToLower());
            }
            if (string.IsNullOrEmpty(movie.Image)) movie.Image = "default.jpg";

            _context.Add(movie);
            await _context.SaveChangesAsync();
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Description");
            return RedirectToAction("Index" , "AdminMovies");
        }

        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                if (id == null || _context.Movies == null)
                {
                    return NotFound();
                }

                var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
                ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Description");
                return View(movie);
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }

       
        [HttpPost]
        public async Task<IActionResult> Edit (int id, Movie movie, Microsoft.AspNetCore.Http.IFormFile fThumb )
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                if (!ModelState.IsValid)
                {
                    try
                    {
                        if (fThumb != null)
                        {
                            string extension = Path.GetExtension(fThumb.FileName);
                            string imageName = Utilities.SEOUrl(movie.Title) + extension;
                            movie.Image = await Utilities.UploadFile(fThumb, @"MoviesImg", imageName.ToLower());
                        }
                        if (string.IsNullOrEmpty(movie.Image)) movie.Image = "default.jpg";
                        _context.Update(movie);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MovieExists(movie.MovieId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "AdminMovies");

                }
                ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Description");
                return View(movie);
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'CenimaDBContext.Movies'  is null.");
            }

            var rateList = await _context.Rates.Where(m => m.MovieId == id).ToListAsync();
            var movieList = await _context.Movies.Where(m => m.MovieId == id).ToListAsync();
            //  var movie = await _context.Movies.FindAsync(id);

            if (rateList != null)
            {
                _context.Rates.RemoveRange(rateList);

                await _context.SaveChangesAsync();
            }
            if (movieList != null)
            {
                _context.Movies.RemoveRange(movieList);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }
    }
}
