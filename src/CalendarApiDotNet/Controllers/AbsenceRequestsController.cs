using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarApiDotNet.Data;
using CalendarApiDotNet.Models;

namespace CalendarApiDotNet.Controllers
{
    [Produces("application/json")]
    [Route("api/AbsenceRequests")]
    public class AbsenceRequestsController : Controller
    {
        //public TodoController(ITodoRepository todoItems)
        //{
        //    TodoItems = todoItems;
        //}
        //public ITodoRepository TodoItems { get; set; }


        private readonly ICalendarRepository _repo;

        public AbsenceRequestsController(ICalendarRepository repository)
        {
            _repo = repository;
        }

        // GET: api/AbsenceRequests
        [HttpGet]
        public IEnumerable<AbsenceRequest> GetAbsenceRequests()
        {
            return _repo.GetAll();
        }

        // GET: api/AbsenceRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbsenceRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AbsenceRequest absenceRequest = await _repo.Find(id);            

            if (absenceRequest == null)
            {
                return NotFound();
            }

            return Ok(absenceRequest);
        }

        //// PUT: api/AbsenceRequests/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAbsenceRequest([FromRoute] int id, [FromBody] AbsenceRequest absenceRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != absenceRequest.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(absenceRequest).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AbsenceRequestExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/AbsenceRequests
        [HttpPost]
        public async Task<IActionResult> PostAbsenceRequest([FromBody] AbsenceRequest absenceRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Add(absenceRequest);            
            try
            {
                await _repo.Save();
            }
            catch (DbUpdateException)
            {
                if (await AbsenceRequestExists(absenceRequest.Id))
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

            AbsenceRequest absenceRequest = await _repo.Remove(id);
            if (absenceRequest == null)
            {
                return NotFound();
            }            
           
            await _repo.Save();

            return Ok(absenceRequest);
        }

        private async Task<bool> AbsenceRequestExists(int id)
        {
            return (await _repo.Find(id)) != null;            
        }
    }
}