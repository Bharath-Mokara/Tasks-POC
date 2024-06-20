using FluentMigratorTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FluentMigratorTask.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public Task<List<User>> Get()
        {
            //fetch all the users from stored procedure
            return _applicationDbContext.Users.FromSqlRaw("EXEC dbo.SP_GetAllUsers").ToListAsync();
            
        }

        
    }
}