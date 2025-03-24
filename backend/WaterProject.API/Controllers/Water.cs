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
}