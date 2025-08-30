using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.DoctorAvailability
{
    public class AvailabilityBaseDTO
    {
        public AvailabilityBaseDTO(DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
        {
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
        [Required(ErrorMessage ="This field is required!")]
        public DayOfWeek DayOfWeek { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public TimeOnly StartTime { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public TimeOnly EndTime { get; set; }
    }
}
