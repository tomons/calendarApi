using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarApi.Models;
using CalendarApiDotNet.Data;

namespace CalendarApiDotNet.Controllers
{
    [Produces("application/json")]
    [Route("api/AbsenceRequests")]
    public class AbsenceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AbsenceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AbsenceRequests
        [HttpGet]
        public IEnumerable<AbsenceRequest> GetAbsenceRequests()
        {
            return _context.AbsenceRequests;
        }

        // GET: api/AbsenceRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbsenceRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AbsenceRequest absenceRequest = await _context.AbsenceRequests.SingleOrDefaultAsync(m => m.Id == id);

            if (absenceRequest == null)
            {
                return NotFound();
            }

            return Ok(absenceRequest);
        }

        // PUT: api/AbsenceRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbsenceRequest([FromRoute] int id, [FromBody] AbsenceRequest absenceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != absenceRequest.Id)
            {
                return BadRequest();
            }

            _context.Entry(absenceRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbsenceRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AbsenceRequests
        [HttpPost]
        public async Task<IActionResult> PostAbsenceRequest([FromBody] AbsenceRequest absenceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AbsenceRequests.Add(absenceRequest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AbsenceRequestExists(absenceRequest.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAbsenceRequest", new { id = absenceRequest.Id }, absenceRequest);
        }

        // DELETE: api/AbsenceRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsenceRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AbsenceRequest absenceRequest = await _context.AbsenceRequests.SingleOrDefaultAsync(m => m.Id == id);
            if (absenceRequest == null)
            {
                return NotFound();
            }

            _context.AbsenceRequests.Remove(absenceRequest);
            await _context.SaveChangesAsync();

            return Ok(absenceRequest);
        }

        private bool AbsenceRequestExists(int id)
        {
            return _context.AbsenceRequests.Any(e => e.Id == id);
        }
    }
}