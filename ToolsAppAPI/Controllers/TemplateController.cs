using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolsAppApi.Data;
using ToolsAppApi.Models;

namespace ToolsAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TemplateController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]Template newTemplate)
        {
            await _applicationDbContext.Templates.AddAsync(newTemplate);
            await _applicationDbContext.SaveChangesAsync();
            Template? templateFromDb = _applicationDbContext.Templates.FirstOrDefault(template=> template.Name == newTemplate.Name);
            if(templateFromDb != null)
            {
                newTemplate.Id = templateFromDb.Id;
            }

            return Ok(new {
                Id = newTemplate.Id,
                Message = "Template added successfully"
            });
        }

        [HttpGet]
        public async Task<List<Template>> GetTemplates()
        {
            return await _applicationDbContext.Templates.ToListAsync();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<Guid> DeleteTemplate(Guid id)
        {
            Template? templateToBeDeleted = _applicationDbContext.Templates.FirstOrDefault(template => template.Id == id);
            if(templateToBeDeleted != null)
            {
                _applicationDbContext.Templates.Remove(templateToBeDeleted);
                await _applicationDbContext.SaveChangesAsync();
            }

            return id;
        }
    }
}