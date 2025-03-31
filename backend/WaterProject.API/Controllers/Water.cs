using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using WaterProject.API.Data;

namespace WaterProject.API.Controllers;

[Route("[controller]")]
[ApiController]

public class WaterController : ControllerBase
{
   private WaterDbContext _waterContext;
   
   public WaterController(WaterDbContext temp)
   {
      _waterContext = temp;
   }

   [HttpGet("AllProjects")]
   public IActionResult GetProjects(int pageHowMany = 10, int pageNum =1, [FromQuery]List<string>? projectTypes = null)
   {
      var query = _waterContext.Projects.AsQueryable();

      if (projectTypes != null && projectTypes.Any())
      {
         query = query.Where(p => projectTypes.Contains(p.ProjectType));
      } 
      
      var totalNumProjects = query.Count();

      var somethings = query
         .Skip((pageNum - 1) * pageHowMany)
         .Take(pageHowMany)
         .ToList();
      
      return Ok(new 
      {
         Projects = somethings,
         totalNumProjects = totalNumProjects
      });
   }
   [HttpGet("GetProjectTypes")]
   public IActionResult GetProjectTypes ()
   {
      var projectTypes = _waterContext.Projects
         .Select(p => p.ProjectType)
         .Distinct()
         .ToList();
      return Ok(projectTypes);
   }

   [HttpPost("AddProject")]
   public IActionResult AddProject([FromBody] Project newProject)
   {
      _waterContext.Projects.Add(newProject);
      _waterContext.SaveChanges();
      return Ok(newProject);
   }

   [HttpPut("UpdateProject/{projectId}")]
   public IActionResult UpdateProject(int projectId, [FromBody] Project updateProject)
   {
      var existingProject = _waterContext.Projects.Find(projectId);

      existingProject.ProjectName = updateProject.ProjectName;
      existingProject.ProjectType = updateProject.ProjectType;
      existingProject.ProjectRegionalProgram = updateProject.ProjectRegionalProgram;
      existingProject.ProjectImpact= updateProject.ProjectImpact;
      existingProject.ProjectPhase= updateProject.ProjectPhase;
      existingProject.ProjectFunctionalityStatus = updateProject.ProjectFunctionalityStatus;

      _waterContext.Projects.Update(existingProject);
      _waterContext.SaveChanges();

      return Ok(existingProject);
   }

   [HttpDelete("DeleteProject/{projectId}")]
   public IActionResult DeleteProject(int projectId) {
      var project =_waterContext.Projects.Find(projectId);

      if (project == null)
      {
         return NotFound (new {message = "project not found"});
      };

      _waterContext.Projects.Remove(project);
      _waterContext.SaveChanges();

      return NoContent();
   }
}