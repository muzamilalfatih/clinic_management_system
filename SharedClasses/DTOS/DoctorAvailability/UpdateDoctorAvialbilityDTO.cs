using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.DoctorAvailability
{
    public class UpdateDoctorAvialbilityDTO : AvailabilityBaseDTO
    { 
        public UpdateDoctorAvialbilityDTO(int id, UpdateAvailabilityRequestDTO updateRequest)
            :base(updateRequest.DayOfWeek, updateRequest.StartTime,updateRequest.EndTime)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
