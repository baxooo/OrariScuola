using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using OrariScuola.Service;
using OrariScuola.Service.Enums;
using System.Text.Json.Serialization;

namespace OrariScuola.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunniController : Controller
    {
        [HttpGet]
        public async Task<FileContentResult> GetCalendar([FromQuery]SectionsEnum section, string? mail)
        {
            var result = await GenerateNewCalendars.Generate(mail, section);

            var file = await System.IO.File.ReadAllBytesAsync(result);

            return File(file, "text/calendar", result.Replace(Directory.GetCurrentDirectory(), ""));
        }
    }
}
