using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Appointment
{
    public class AddNewAppointmentDTO
    {
        public AddNewAppointmentDTO(int patientId, int doctorId, decimal fee, DateTime date, string? notes, int? parentAppointmentId)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            Fee = fee;
            Date = date;
            Notes = notes;
            ParentAppointmentId = parentAppointmentId;
        }

        [Required(ErrorMessage = "PatientId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a positive number.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a positive number.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Fee is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fee must be greater than zero.")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime Date { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ParentAppointmentId must be a positive number if provided.")]
        public int? ParentAppointmentId { get; set; }
    }
}
