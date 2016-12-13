using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Models
{
    public class AbsenceRequestDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }        
        //todo: make it to be only date, without time part
        public DateTime FromDate { get; set; }
        //todo: make it to be only date, without time part
        public DateTime ToDate { get; set; }
        public AbsenceRequestState State { get; set; }
    }

    public class AbsenceRequestCreateDto
    {
        //todo: make it to be only date, without time part     
        public DateTime FromDate { get; set; }
        //todo: make it to be only date, without time part
        public DateTime ToDate { get; set; }        
    }
}
