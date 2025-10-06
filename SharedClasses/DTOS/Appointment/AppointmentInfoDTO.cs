using SharedClasses.Enums;
using System.ComponentModel.DataAnnotations;

namespace SharedClasses.DTOS.Appointment
{


    public class AppointmentInfoDTO
    {
        public AppointmentInfoDTO(int id, string patient, string doctor, decimal fee, DateTime date, AppointmentStatus status,
            string? notes, string? symptoms, string? diagnoses, int? parentAppointmentId)
        {
            Id = id;
            Patient = patient;
            Doctor = doctor;
            Fee = fee;
            Date = date;
            Status = status;
            Notes = notes;
            Symptoms = symptoms;
            Diagnoses = diagnoses;
            ParentAppointmentId = parentAppointmentId;
        }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than zero.")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(200, ErrorMessage = "Patient name can't be longer than 200 characters.")]
        public string Patient { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(200, ErrorMessage = "Doctor name can't be longer than 200 characters.")]
        public string Doctor { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Fee must be a positive number.")]
        public decimal Fee { get; set; }

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime Date { get; set; }

        [Required]
        [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Invalid appointment status.")]
        public AppointmentStatus Status { get; set; }

        [StringLength(500, ErrorMessage = "Notes can't be longer than 500 characters.")]
        public string? Notes { get; set; }
        public string? Symptoms { get; set; }
        public string? Diagnoses { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ParentAppointmentId must be greater than zero.")]
        public int? ParentAppointmentId { get; set; }
       
    }
}
