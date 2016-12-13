using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarApiDotNet.Models
{
    public class AbsenceRequest
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
                
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
                
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
                
        public AbsenceRequestState State { get; set; }
    }
}
