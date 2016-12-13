using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Models
{
    public class AbsenceRequestDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AbsenceRequestState State { get; set; }
    }

    public class AbsenceRequestCreateDto
    {           
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }        
    }
}
