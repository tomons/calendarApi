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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CalendarApiDotNet.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/AbsenceRequests")]
    public class AbsenceRequestsController : Controller
    {
        private readonly ICalendarRepository _repo;
        private readonly UserManager<ApplicationUser> _manager;
        private readonly IMapper _mapper;

        public AbsenceRequestsController(
            ICalendarRepository repository,
            UserManager<ApplicationUser> manager,
            IMapper mapper)
        {
            _repo = repository;
            _manager = manager;
            _mapper = mapper;
        }

        // GET: api/AbsenceRequests
        [Authorize(Roles = SeedData.AdminRole)]
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

        // PUT: api/AbsenceRequests/approve/5
        [Authorize(Roles = SeedData.AdminRole)]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> PutAbsenceRequest([FromRoute] int id)
        {
            return await UpdateRequestState(id, AbsenceRequestState.Approved);
        }

        // PUT: api/AbsenceRequests/approve/5
        [Authorize(Roles = SeedData.AdminRole)]
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> PutAbsenceRequestReject([FromRoute] int id)
        {
            return await UpdateRequestState( id, AbsenceRequestState.Requested);
        }

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
                UserId = GetCurrentUserId()
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

        private async Task<IActionResult> UpdateRequestState(
           int id,          
           AbsenceRequestState newState
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var request = await _repo.Find(id);
                if (request == null)
                {
                    return NotFound();
                }              

                request.State = newState;
                await _repo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AbsenceRequestExists(id))
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

        private async Task<bool> AbsenceRequestExists(int id)
        {
            return (await _repo.Find(id)) != null;            
        }

        private string GetCurrentUserId()
        {
            return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}