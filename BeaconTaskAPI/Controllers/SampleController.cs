using BeaconTaskAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeaconTaskAPI
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SampleController : ControllerBase
    {
        [HttpPost("release-Access")]
        public async Task<IActionResult> ReleaseAccess(ReleaseData releaseData)
        {
            Thread.Sleep(5000);
            Console.WriteLine("Lock Released Successfully");
            //Perform the DB Operation here to release lock
            
            return Ok(new { Success = true });
        }
    }
}