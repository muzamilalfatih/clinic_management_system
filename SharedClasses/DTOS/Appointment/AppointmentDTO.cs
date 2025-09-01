using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Appointment
{
    public class AppointmentDTO
    {
        public AppointmentDTO(int id, int patientId, int doctorId, 
            decimal fee, int billId, DateTime date, AppointmentStatus status, string? notes, int? parentAppoinmentId)
        {
            Id = id;
            PatientId = patientId;
            DoctorId = doctorId;
            Fee = fee;
            BillId = billId;
            Date = date;
            Status = status;
            Notes = notes;
            ParentAppointmentId = parentAppoinmentId;
        }

        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "PatientId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a positive number.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a positive number.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Fee is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fee must be greater than zero.")]
        public decimal Fee { get; set; }

    
        [Range(1, int.MaxValue, ErrorMessage = "BillId must be a positive number.")]
        public int? BillId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Invalid appointment status.")]
        public AppointmentStatus Status { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ParentAppointmentId must be a positive number if provided.")]
        public int? ParentAppointmentId { get; set; }
    }
}
