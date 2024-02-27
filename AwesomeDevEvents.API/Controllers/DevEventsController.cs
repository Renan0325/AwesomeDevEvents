using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;

        public DevEventsController(DevEventsDbContext context) { 
            _context = context;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            var devEvents = _context.DevEvents.Where(d=> !d.isDelete).ToList();
            return Ok(devEvents);
        }
        
        [HttpGet("{id}")]
        public IActionResult getById(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); //Faz com o que eu trago o primeiro realizando o filtro.
            if(devEvent == null)
            {
                return NotFound();
            }
            return Ok(devEvent);
        }

        [HttpPost]
        public IActionResult Post([FromBody] DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            return CreatedAtAction(nameof(getById), new { id = devEvent.Id}, devEvent);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); 
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); 
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Delete();
            return NoContent();
        }

        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id,DevEventSpeaker speaker)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Speakers.Add(speaker);
            return NoContent();
        }

    }
}
