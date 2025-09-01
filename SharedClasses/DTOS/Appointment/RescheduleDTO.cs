using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Appointment
{
    public class RescheduleDTO
    {
        public RescheduleDTO(int id, DateTime newDate)
        {
            Id = id;
            NewDate = newDate;
        }

        [Required(ErrorMessage = "Appointment Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Appointment Id must be greater than 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "New date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime NewDate { get; set; }
    }
}
