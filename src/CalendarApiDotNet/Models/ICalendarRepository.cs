﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Models
{
    public interface ICalendarRepository
    {
        void Add(AbsenceRequest item);
        IEnumerable<AbsenceRequest> GetAll(Func<AbsenceRequest, bool> condition);
        Task<AbsenceRequest> Find(int id);
        Task<AbsenceRequest> Remove(int id);
        Task Save();
    }
}
