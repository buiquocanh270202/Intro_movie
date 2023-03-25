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
    public class AdminAccountController : Controller
    {
        private readonly CenimaDBContext _context;

        public AdminAccountController(CenimaDBContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                var person = _context.Persons.Where(x => x.Type == 2).ToList();
                return View(person);
            } else 
            {
                return Redirect("~/PersonLogin/Login");
            }
            
        }


        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                if (id == null || _context.Persons == null)
                {
                    return NotFound();
                }

                var person = _context.Persons.Find(id);

                return View(person);
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, Person person)
        {
            if (HttpContext.Session.GetObjectFromJson<LoginModel>("admin") != null)
            {
                if (id != person.PersonId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(person);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PersonExists(person.PersonId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(person);
            }
            else
            {
                return Redirect("~/PersonLogin/Login");
            }
        }


        private bool PersonExists(int id)
        {
            return (_context.Persons?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}