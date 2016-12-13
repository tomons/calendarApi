using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Models
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Create()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<AbsenceRequest, AbsenceRequestDto>());
        }
    }
}
