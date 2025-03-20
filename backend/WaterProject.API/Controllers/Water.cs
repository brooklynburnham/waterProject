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
   public IActionResult GetProjects(int pageHowMany = 10, int pageNum =1)
   {
      var somethings = _waterContext.Projects
         .Skip((pageNum - 1) * pageHowMany)
         .Take(pageHowMany)
         .ToList();
      
      var totalNumProjects = _waterContext.Projects.Count();
      
      return Ok(new 
      {
         Projects = somethings,
         totalNumProjects = totalNumProjects
      });
   }

   [HttpGet("FunctionalProjects")]
   public IEnumerable<Project> GetFunctionalProjects()
   {
      var something = _waterContext.Projects.Where(p => p.ProjectFunctionalityStatus == "Functional").ToList();
      return something;
   }
}