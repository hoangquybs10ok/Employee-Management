using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Entity;
using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using EmployeeManagement.TestDb;

namespace EmployeeManagement.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly DbContextTest _context;

        public UserController(DbContextTest context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString("UserName");
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var users = _userRepository.GetAll();
            return View(users);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewBag.Projects = new SelectList(_context.Projects.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = new SelectList(_context.Projects.ToList(), "Id", "Name");

                return View(model);
            }

            _userRepository.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();

            var model = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                ProjectIds = user.UserProjects.Select(p => p.ProjectId).ToList()
            };

            ViewBag.Projects = new MultiSelectList(_context.Projects.ToList(), "Id", "Name", model.ProjectIds);
            return View(model);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = new MultiSelectList(_context.Projects.ToList(), "Id", "Name", model.ProjectIds);
                return View(model);
            }
            _userRepository.Edit(model);
            return RedirectToAction("Index");
        }

        [HttpGet("details/{id}")]
        public IActionResult Detail(int id)
        {
            var model = _userRepository.Detail(id);
            if (model == null) return NotFound();

            var allProjects = _context.Projects
            .Select(p => new ProjectModel {
                Id = p.Id,
                Name = p.Name 
            }).ToList();
            ViewBag.Projects = allProjects;
            return View(model);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _userRepository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new UserModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FullName = entity.FullName,
                Email = entity.Email,
                Password = entity.PasswordHash,
                Projects = entity.UserProjects.Select(x => new ProjectModel()
                {
                    Id = x.ProjectId,
                    Name = x.Project.Name
                }).ToList()
            };
           
            return View(model);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _userRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
