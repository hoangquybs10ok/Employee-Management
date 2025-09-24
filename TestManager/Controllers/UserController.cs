using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManager.Entity;
using TestManager.Models;
using TestManager.TestDb;

namespace TestManager.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly DbContextTest _context;
        public UserController(DbContextTest context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var secsion = HttpContext.Session.GetString("UserName");
            if (secsion == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var users = _context.Users.ToList();
            return View(users);
        }
        [HttpGet("create")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(UserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new UserEntity
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            var model = new UserModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,


            };
            return View(model);
        }
        [HttpPost("edit/{id}")]
        public IActionResult Edit(UserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = _context.Users.Find(model.Id);
            if (user == null)
                return NotFound();

            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Email = model.Email;
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet("details/{id}")]
        public IActionResult Detail(int id)
        {

            var user = _context.Users.
                Where(x => x.Id == id).
                Select(x => new UserModel()
                {
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Email = x.Email,
                }).FirstOrDefault();
            return View(user);
        }
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
