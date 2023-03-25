using Microsoft.AspNetCore.Mvc;

using Project_Movie_Group6.DAO;
using Project_Movie_Group6.Models;

namespace Project_Movie_Group6.Controllers
{
    using Microsoft.AspNetCore.Session;
    using Microsoft.EntityFrameworkCore;
    using Project_Movie_Group6.Helpper;
    using System.Drawing;
    using System.Text.Json;

    public class PersonLoginController : Controller
    {


        private readonly CenimaDBContext _context;

        public PersonLoginController(CenimaDBContext context)
        {
            _context = context;
        }

        

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpModel person)
        {
            if (ModelState.IsValid)
            {
                


                var acc = _context.Persons.SingleOrDefault(a => a.Email.Equals(person.Email));
                if (acc != null)
                {
                    ViewData["msg"] = "Email is exsit! !";
                    return View();
                }
                if (person.Password =="" || person.Password.Equals(""))// signupmodel
                {
                    ViewData["msg"] = "Password Not Empty!";
                    return View();
                }


                var pers = new Person
                {
                    Fullname = person.Fullname,
                    Email = person.Email,
                    Gender = person.Gender,
                   
                    Password = person.Password,
                    Type = 2,
                    IsActive = true
                };

                _context.Persons.Add(pers);

                _context.SaveChanges();
               
                HttpContext.Session.SetString("account", JsonSerializer.Serialize(pers));

                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
                /*return RedirectToPage("index");*/
            }
            else
            {
                return View();
            }
        }

        
    
           
        



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)// kiểm tr form rỗng 
            {
                var dao = new PersonDAO(_context);
                
                var result = dao.Login(model.Email, model.Password);
                if (result)
                {
                    var person = dao.GetByEmail(model.Email);

                    if (person.IsActive == true)
                    {
                        if (person.Type == 2)
                        {
                            HttpContext.Session.SetString("account", JsonSerializer.Serialize(model));
                            Person p = HttpContext.Session.GetObjectFromJson<Person>("account");
                            
                            return RedirectToAction("Index", "Home"); // vào trang home voi action index
                        }
                        else
                        {
                            HttpContext.Session.SetString("admin", JsonSerializer.Serialize(model));
                            
                            return Redirect("~/Admin"); // vào trang home voi action index
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Account Is Blocked!!!");
                    }
                        
                    
                }
                else
                {
                    ModelState.AddModelError("", "Login Fail!!!");
                }

            }
            return Login();


        }

        public IActionResult SignOut()
        {
            if (HttpContext.Session.GetString("account") != null) { HttpContext.Session.Remove("account"); }
                
            if (HttpContext.Session.GetString("admin") != null) { HttpContext.Session.Remove("admin"); }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
