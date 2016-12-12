using CalendarApi.Models;
using CalendarApiDotNet.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarApi.Models
{
    public class AbsenceRequest
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public AbsenceRequestState State { get; set; }
    }
}
