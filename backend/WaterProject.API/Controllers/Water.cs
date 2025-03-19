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
   public IEnumerable<Project> GetProjects()
   {
      var somethings = _waterContext.Projects
         .Take(5)
         .ToList();
      
      return somethings;
   }

   [HttpGet("FunctionalProjects")]
   public IEnumerable<Project> GetFunctionalProjects()
   {
      var something = _waterContext.Projects.Where(p => p.ProjectFunctionalityStatus == "Functional").ToList();
      return something;
   }
}