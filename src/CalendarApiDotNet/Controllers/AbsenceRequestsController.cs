using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarApiDotNet.Data;
using CalendarApiDotNet.Models;
using AutoMapper;

namespace CalendarApiDotNet.Controllers
{
    [Produces("application/json")]
    [Route("api/AbsenceRequests")]
    public class AbsenceRequestsController : Controller
    {
        private readonly ICalendarRepository _repo;
        private readonly IMapper _mapper;

        public AbsenceRequestsController(
            ICalendarRepository repository,
            IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        // GET: api/AbsenceRequests
        [HttpGet]
        public IEnumerable<AbsenceRequestDto> GetAbsenceRequests()
        {
            return _mapper.Map<IEnumerable<AbsenceRequestDto>>(_repo.GetAll());
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

            return Ok(_mapper.Map<AbsenceRequestDto>(absenceRequest));
        }

        // todo: Approve, Reject request

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
        public async Task<IActionResult> PostAbsenceRequest([FromBody] AbsenceRequestCreateDto absenceRequestCreateDto)
        {
            //todo: validation (dates not in past etc.)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newAbsenceRequest = new AbsenceRequest()
            {
                CreatedAt = DateTime.UtcNow,
                FromDate = absenceRequestCreateDto.FromDate,
                ToDate = absenceRequestCreateDto.ToDate,
                State = AbsenceRequestState.Requested,
                UserId = null // todo: set current user id
            };            
            _repo.Add(newAbsenceRequest);            
            try
            {
                await _repo.Save();
            }
            catch (DbUpdateException)
            {
                if (await AbsenceRequestExists(newAbsenceRequest.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAbsenceRequest", new { id = newAbsenceRequest.Id }, _mapper.Map<AbsenceRequestDto>(newAbsenceRequest));
        }

        //todo: probably will not need exposed delete method

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

            return Ok(_mapper.Map<AbsenceRequestDto>(absenceRequest));
        }

        private async Task<bool> AbsenceRequestExists(int id)
        {
            return (await _repo.Find(id)) != null;            
        }
    }
}