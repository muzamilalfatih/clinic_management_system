using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Appointment
{
    public class AppointmentFilterDTO
    {
        public AppointmentFilterDTO(int pageNumber, int pageSize, int? doctorId, int? patientId, DateTime? startDate,
            DateTime? endDate, AppointmentStatus? status)
        {
            DoctorId = doctorId;
            PatientId = patientId;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a positive number.")]
        public int? DoctorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a positive number.")]
        public int? PatientId { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid StartDate format.")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid EndDate format.")]
        public DateTime? EndDate { get; set; }

        [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Invalid appointment status.")]
        public AppointmentStatus? Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than zero.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = 20;
    }
}
