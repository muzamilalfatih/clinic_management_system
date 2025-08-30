using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.DoctorAvailability
{
    public class CreateAvailabilityRequestDTO : AvailabilityBaseDTO
    {
        public CreateAvailabilityRequestDTO(DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
            :base(dayOfWeek,startTime,endTime)
        {
        }

    }
}
