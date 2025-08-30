using SharedClasses.DTOS.Doctors;
using SharedClasses.DTOS.LabTechnician;
using SharedClasses.DTOS.Patients;
using SharedClasses.DTOS.People;
using SharedClasses.DTOS.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Authentication
{
    public class ResponseProfileDTO
    {
        public ResponseProfileDTO(UserProfileDTO user,PersonProfileDTO person,
            PatientProfileDTO patient = null, DoctorProfileDTO doctor = null, LabTechnicianProfileDTO labTechnician = null)
        {
            this.user = user;
            this.patient = patient;
            this.doctor = doctor;
            this.labTechnician = labTechnician;
            this.person = person;
        }

        public UserProfileDTO user {  get; set; }
        public PatientProfileDTO patient { get; set; }
        public DoctorProfileDTO doctor { get; set; }
        public LabTechnicianProfileDTO labTechnician { get; set; }
        public PersonProfileDTO person { get; set; }

    }
}
