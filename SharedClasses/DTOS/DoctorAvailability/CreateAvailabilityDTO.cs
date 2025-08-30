using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.DoctorAvailability
{
    public class CreateAvailabilityDTO : AvailabilityBaseDTO
    {
        public CreateAvailabilityDTO(int doctorId, CreateAvailabilityRequestDTO createRequestDTO)
            :base(createRequestDTO.DayOfWeek,createRequestDTO.StartTime,createRequestDTO.EndTime)
        {
            DoctorId = doctorId;
        }

        public int DoctorId { get; set; }
    }
}
