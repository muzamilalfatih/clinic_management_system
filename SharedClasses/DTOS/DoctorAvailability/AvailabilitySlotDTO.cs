using SharedClasses.DTOS.DoctorAvailability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class AvailabilitySlotDTO : AvailabilityBaseDTO
    {
        public AvailabilitySlotDTO(DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
            : base(day, startTime, endTime)
        {
        }

    }
}
