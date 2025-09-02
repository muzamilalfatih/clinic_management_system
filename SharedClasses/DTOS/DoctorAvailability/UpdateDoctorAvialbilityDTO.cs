using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.DoctorAvailability
{
    public class UpdateDoctorAvialbilityDTO : AvailabilityBaseDTO
    { 
        public UpdateDoctorAvialbilityDTO(int id, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
            :base(dayOfWeek, startTime, endTime)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
