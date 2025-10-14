using EmployeeManagement.EF.Entity;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EmployeeManagement.Controllers
{
    [Route("project")]
    [Authorize(Roles = "Admin,HR,Manager")]
    public class ProjectController : BaseController
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var projects = _projectRepository.GetAll().Select(x => new ProjectModel
            {
                Id =x.Id,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,

            }).ToList();
           
            return View(projects);
        }
        [HttpGet("create")]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(ProjectModel model)
        {
            var entity = new ProjectEntity
            {
                Name = model.Name ?? string.Empty,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
            };
            _projectRepository.Create(entity);
            _projectRepository.Save();
            return RedirectToAction("Index");
        }
        [HttpGet("detail/{id}")]
        public IActionResult Detail(int id)
        {
            var project = _projectRepository.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
            };
            return View(model);
        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var entity = _projectRepository.GetById(id);
            if(entity == null)
            {
                return NotFound();
            }
            var model = new ProjectModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
            };
            
            return View(model);
        }
        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, ProjectModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Employyee = _projectRepository.GetAll();
                return View(model);
            }
            var entity = new ProjectEntity
            {
                Id = model.Id,
                Name = model.Name!,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
            };
            _projectRepository.Edit(entity);
            _projectRepository.Save();
            return RedirectToAction("Index");
        }
        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var project = _projectRepository.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            var model = new ProjectModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };
            return View(model);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _projectRepository.Delete(id);
            _projectRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
