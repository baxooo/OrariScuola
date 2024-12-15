using Microsoft.AspNetCore.Mvc;
using OrariScuola.Service;
using OrariScuola.Service.Enums;

namespace OrariScuola.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunniController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<FileContentResult>> GetCalendar([FromQuery]SectionsEnum section, string? mail)
        {
            var result = await GenerateNewCalendars.GenerateStudent(mail, section);

            if (string.IsNullOrEmpty(result))
                return BadRequest("File Not Found");
            
            var file = await System.IO.File.ReadAllBytesAsync(result);

            return Ok(File(file, "text/calendar", result.Replace(Directory.GetCurrentDirectory(), "")));
        }
    }
}
