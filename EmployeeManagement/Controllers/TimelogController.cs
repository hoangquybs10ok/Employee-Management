using EmployeeManagement.EF.Entity;
using EmployeeManagement.EF.Entity.Enums;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Route("timelog")]
    [Authorize(Roles = "Admin,HR,Manager,Employee")]
    public class TimelogController : Controller
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IUserRepository _userRepository;

        public TimelogController(ITimeLogRepository timeLogRepository, IUserRepository userRepository)
        {
            _timeLogRepository = timeLogRepository;
            _userRepository = userRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue("Id") ?? "0");

            var timelogs = _timeLogRepository.GetAll();

            // Nếu là Employee chỉ xem TimeLog của chính mình
            if (role == "Employee")
            {
                timelogs = timelogs.Where(x => x.UserId == userId);
            }

            var result = timelogs.Select(x => new TimeLogModel
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User!.UserName,
                CheckIn = x.CheckIn,
                CheckOut = x.CheckOut,
                Description = x.Description,
                Task = x.Task,
            }).ToList();

            return View(result);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue("Id") ?? "0");

            // Nếu là employee chỉ hiển thị chính họ trong ViewBag
            if (role == "Employee")
            {
                var currentUser = _userRepository.GetById(userId);
                var employees = new List<UserEntity>();
                if (currentUser != null)
                {
                    employees.Add(currentUser);
                }
                ViewBag.Employee = employees;
            }
            else
            {
                ViewBag.Employee = _userRepository.GetAll();
            }

            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TimeLogModel model)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            if (!ModelState.IsValid)
            {
                ViewBag.Employee = _userRepository.GetAll();
                return View(model);
            }


            // Employee chỉ được thêm TimeLog của chính mình
            if (role == RoleType.Employee.ToString())
            {
                model.UserId = userId;
            }

            _timeLogRepository.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet("detail/{id}")]
        public IActionResult Detail(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            var timelog = _timeLogRepository.Detail(id);
            if (timelog == null)
                return NotFound();

            // Employee chỉ xem chi tiết log của chính mình
            if (role == "Employee" && timelog.UserId != userId)
                return RedirectToAction("AccessDenied", "Account");

            return View(timelog);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            var entity = _timeLogRepository.GetById(id);
            if (entity == null)
                return NotFound();


            // Employee chỉ được sửa TimeLog của chính mình
            if (role == "Employee" && entity.UserId != userId)
                return RedirectToAction("AccessDenied", "Account");

            var model = new TimeLogModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut,
                Description = entity.Description,
                Task = entity.Task,
            };

            var employees = new List<UserEntity>();
            if (role == "Employee")
            {
                var currentUser = _userRepository.GetById(userId);
                if (currentUser != null)
                    employees.Add(currentUser);
            }
            else
            {
                employees = _userRepository.GetAll().ToList();
            }

            ViewBag.Employee = employees;

            return View(model);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TimeLogModel model)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            if (!ModelState.IsValid)
            {
                ViewBag.Employee = _userRepository.GetAll();
                return View(model);
            }

            var entity = _timeLogRepository.GetById(id);

            if (entity == null)
                return NotFound();

            if (role == "Employee" && entity.UserId != userId)
                return RedirectToAction("AccessDenied", "Account");

            _timeLogRepository.Edit(model);
            return RedirectToAction("Index");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            var entity = _timeLogRepository.GetById(id);
            if (entity == null)
                return NotFound();


            if (role == "Employee" && entity.UserId != userId)
                return RedirectToAction("AccessDenied", "Account");

            var model = new TimeLogModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut,
                Description = entity.Description,
                Task = entity.Task,
            };
            return View(model);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var entity = _timeLogRepository.GetById(id);
            if (entity == null)
                return NotFound();

            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue("Id") ?? "0");

            if (role == "Employee" && entity.UserId != userId)
                return RedirectToAction("AccessDenied", "Account");

            _timeLogRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
