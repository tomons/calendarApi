using CalendarApiDotNet.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Models
{
    public class CalendarRepository : ICalendarRepository
    {
        private ApplicationDbContext _context;

        public CalendarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AbsenceRequest absenceRequest)
        {
            _context.AbsenceRequests.Add(absenceRequest);
        }

        public async Task<AbsenceRequest> Find(int id)
        {
           return await _context.AbsenceRequests.SingleOrDefaultAsync(m => m.Id == id);
        }

        public IEnumerable<AbsenceRequest> GetAll(Func<AbsenceRequest,bool> condition = null)
        {
            if (condition == null) return _context.AbsenceRequests;
            return _context.AbsenceRequests.Where(condition);
        }

        public async Task<AbsenceRequest> Remove(int id)
        {
            var absenceRequest = await Find(id);
            if (absenceRequest == null) return null;
            _context.AbsenceRequests.Remove(absenceRequest);
            return absenceRequest;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
