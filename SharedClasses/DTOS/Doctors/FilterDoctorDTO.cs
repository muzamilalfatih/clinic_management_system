using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class FilterDoctorDTO
    {
        public FilterDoctorDTO() { }
        public FilterDoctorDTO(int pageNumber, int pageSize,int? specializationId , DayOfWeek? dayOfWeek , TimeSpan? time)
        {
            SpecializationId = specializationId;
            DayOfWeek = dayOfWeek;
            Time = time;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int? SpecializationId { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public TimeSpan? Time {  get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
