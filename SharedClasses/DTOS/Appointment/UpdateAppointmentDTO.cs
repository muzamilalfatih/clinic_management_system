using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Appointment
{
    public class UpdateAppointmentDTO
    {
        public UpdateAppointmentDTO(int id, int doctorId, decimal fee, DateTime date,
            string? notes, int? parentAppointmentId, string? symptoms, string? diagnoses)
        {
            Id = id;
            DoctorId = doctorId;
            Fee = fee;
            Date = date;
            Notes = notes;
            ParentAppointmentId = parentAppointmentId;
            Symptoms = symptoms;
            Diagnoses = diagnoses;
        }

        [Required(ErrorMessage = "Appointment Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Appointment Id must be greater than 0.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Doctor Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Doctor Id must be greater than 0.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Fee is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fee must be greater than 0.")]
        public decimal Fee { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime Date { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Parent Appointment Id must be greater than 0.")]
        public int? ParentAppointmentId { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnoses { get; set; }
    }
}
