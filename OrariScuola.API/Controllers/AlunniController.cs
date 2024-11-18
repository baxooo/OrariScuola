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
        public async Task<FileContentResult> GetCalendar([FromQuery]SectionsEnum section)
        {
            //TODO - ShouldUpdate() method returns a bool, so:
            // if(Class.ShouldUpdate())
            // {
            //     await GenerateNewCalendars.Generate()
            // }
            // this way, if we have files to update we update them and then have them ready to go

            var result = await GenerateNewCalendars.Generate(true, section);

            var file = await System.IO.File.ReadAllBytesAsync(result);

            return File(file, "text/calendar", result.Replace(Directory.GetCurrentDirectory(), ""));
        }
    }
}
