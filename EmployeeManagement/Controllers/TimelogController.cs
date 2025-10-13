using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.EF.Entity;
using EmployeeManagement.EF.Repository.Interface;

namespace EmployeeManagement.Controllers
{
    [Route("timelog")]
    public class TimelogController : BaseController
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IUserRepository _userRepository;
        public TimelogController(ITimeLogRepository timeLogRepository, IUserRepository userRepository)
        {
            _timeLogRepository = timeLogRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            var timelogs = _timeLogRepository.GetAll()
                .Select(x => new TimeLogModel
            {
                Id = x.Id,
                UserId = x.UserId,
                CheckIn = x.CheckIn,
                CheckOut = x.CheckOut,
                Description = x.Description,
                Task = x.Task,
            }).ToList();
            
            return View(timelogs);
        }
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewBag.Employee = _userRepository.GetAll();
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(TimeLogModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employee = _userRepository.GetAll();
                return View(model);
            }
            _timeLogRepository.Create(model);
            return RedirectToAction("Index");
        }
        [HttpGet("detail/{id}")]
        public IActionResult Detail(int id)
        {
            var timelog = _timeLogRepository.Detail(id);
            if (timelog == null)
                return NotFound();

            return View(timelog);
        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var entity = _timeLogRepository.GetById(id);
            if(entity == null)
            {
                return NotFound();
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
            if (!ModelState.IsValid)
            {
                ViewBag.Employee = _userRepository.GetAll();
                return View(model);
            }

            _timeLogRepository.Edit(model);
            return RedirectToAction("Index");
        }
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _timeLogRepository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new TimeLogModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CheckIn = entity.CheckIn,
                CheckOut = entity.CheckOut,
                Description =entity.Description,
                Task = entity.Task,
            };
            return View(model);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _timeLogRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
