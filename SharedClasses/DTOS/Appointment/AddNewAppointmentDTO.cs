using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Appointment
{
    public class AddNewAppointmentDTO
    {
        public AddNewAppointmentDTO(AddNewAppointmentRequestDTO addNewAppointmentRequestDTO, decimal fee, int billId)
        {
            PatientId = addNewAppointmentRequestDTO.PatientId;
            DoctorId = addNewAppointmentRequestDTO.DoctorId;
            Fee = fee;
            BillId = billId;
            Date = addNewAppointmentRequestDTO.Date;
            Notes = addNewAppointmentRequestDTO.Notes;
            ParentAppointmentId = addNewAppointmentRequestDTO.ParentAppointmentId;
        }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public decimal Fee { get; set; }
        public int BillId { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public int? ParentAppointmentId { get; set; }      
    }
}
