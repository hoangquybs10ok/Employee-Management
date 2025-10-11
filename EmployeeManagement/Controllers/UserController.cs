using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.EF.TestDb;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeManagement.EF.Entity.Enums;


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

        public IActionResult Index(RoleType? roleFiler)
        {
            var session = HttpContext.Session.GetString("UserName");
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var users = _userRepository.GetAll();
            if (roleFiler.HasValue)
            {
                users = users.Where(u => u.Role == roleFiler.Value).ToList();
            }
            var currentRole = HttpContext.Session.GetString("UserRole");
            ViewBag.CurrentRole = currentRole;
            ViewBag.Roles = Enum.GetValues(typeof(RoleType))
                .Cast<RoleType>().Select(r => new SelectListItem
                {
                    Text = r.ToString(),
                    Value = r.ToString()
                });
            return View(users);
        }
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == RoleType.Admin.ToString();
        }
        private bool IsMember()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == RoleType.Employee.ToString() ||
                   role == RoleType.Manager.ToString()  ||
                   role == RoleType.HR.ToString();
        }
        [HttpGet("create")]
        public IActionResult Create()
        { 
            if(!IsAdmin())
                return Forbid(); //chi admin dc tao
            ViewBag.Projects = new SelectList(_context.Projects.ToList(), "Id", "Name");
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(EmployeeManagement.EF.Entity.Enums.RoleType)));
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(UserModel model)
        {
            if (!IsAdmin()) 
                return Forbid(); // kiem tra quyen
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
            if (!IsAdmin()) 
                return Forbid();
            var user = _userRepository.GetById(id);
            if (user == null) return NotFound();

            var model = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                ProjectIds = user.UserProjects.Select(p => p.ProjectId).ToList()
            };

            ViewBag.Projects = new MultiSelectList(_context.Projects.ToList(), "Id", "Name", model.ProjectIds);
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(EmployeeManagement.EF.Entity.Enums.RoleType)), model.Role);
            return View(model);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(UserModel model)
        {
            if (!IsAdmin())
                return Forbid();
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
            var user = _userRepository.GetById(id);
            if (user == null)
                return NotFound();

            var model = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = user.Role, // hoặc user.Role.ToString() nếu Role trong UserModel là string
                ProjectIds = user.UserProjects?.Select(up => up.ProjectId).ToList() ?? new List<int>()
            };

            ViewBag.Projects = _context.Projects
                .Select(p => new ProjectModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();

            return View(model);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
                return Forbid();
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
                    Name = x.Project!.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return Forbid();
            _userRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
