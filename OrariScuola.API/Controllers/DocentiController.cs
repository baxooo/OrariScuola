using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrariScuola.Service;
using OrariScuola.Service.Enums;
using static System.Collections.Specialized.BitVector32;

namespace OrariScuola.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DocentiController : Controller
{
    [HttpGet]
    public async Task<ActionResult<FileContentResult>> GetCalendar([FromQuery]ProfessorsEnum prof, string? mail)
    {
        var result = await GenerateNewCalendars.GenerateProfessor(mail, prof);

        if (string.IsNullOrEmpty(result))
            return BadRequest("File Not Found");

        var file = await System.IO.File.ReadAllBytesAsync(result);

        return Ok(File(file, "text/calendar", result.Replace(Directory.GetCurrentDirectory(), "")));
    }
}
