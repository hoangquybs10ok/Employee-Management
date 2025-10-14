using EmployeeManagement.EF.Entity;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Route("timelog")]
    [Authorize(Roles = "Admin,HR,Manager,Employee")]
    public class TimelogController : BaseController
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
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            var allLogs = _timeLogRepository.GetAll()
                .Select(x => new TimeLogModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    CheckIn = x.CheckIn,
                    CheckOut = x.CheckOut,
                    Description = x.Description,
                    Task = x.Task,
                }).ToList();
            if (role == "Employee")
            {
                allLogs = allLogs.Where(x => x.UserId == userId).ToList();
            }
            return View(allLogs);
        }
        [HttpGet("create")]
        public IActionResult Create()
        {
            // Employee tạo log của chính họ 
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Employee")
            {
                ViewBag.Employee = _userRepository.GetAll().Where(u => u.UserName == User.Identity!.Name);
            }
            else
            {
                ViewBag.Employee = _userRepository.GetAll();
            }
            return View();
        }
        [HttpPost("create")]
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
            if (role == "Employee")
            {
                model.UserId = userId; // ép userId hiện tại vào để nhân viên chỉ tạo log cho chính họ
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
            if (role == "Employee" && timelog.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
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
            {
                return NotFound();
            }
            if (role == "Employee" && entity.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var model = new TimeLogModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut,
                Description = entity.Description,
                Task = entity.Task,
            };
            ViewBag.Employee = _userRepository.GetAll();
            return View(model);
        }
        [HttpPost("edit/{id}")]
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
            if(role == "Employee" && model.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
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
            {
                return NotFound();
            }
            if (role == "Employee" && entity.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
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
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue("Id");
            int.TryParse(userIdClaim, out int userId);
            var entity = _timeLogRepository.GetById(id);
            if(entity == null)
            {
                return NotFound();
            }
            if(role == "Employee" && entity.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            _timeLogRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
