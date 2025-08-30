using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class DoctorInfoDTO
    {
        public DoctorInfoDTO(int id, string name, string specialization, byte prevExperienceYears,
            DateTime joinDate, string bio, decimal consulationFee, List<AvailabilitySlotDTO> availabilities)
        {

            this.specialization = specialization;
            this._prevExperienceYears = prevExperienceYears;
            this._joinDate = joinDate;
            this.bio = bio;
            this.consulationFee = consulationFee;
            this.id = id;
            this.Availabilities = availabilities;
            this.name = name;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string specialization {  get; set; }
        public int experienceYears => _prevExperienceYears + ((DateTime.Now - _joinDate).Days / 350);
        private int _prevExperienceYears {  get; set; }
        private DateTime _joinDate {  get; set; }
        public string bio {  get; set; }
        public decimal consulationFee { get; set; }
        public List<AvailabilitySlotDTO> Availabilities { get; set; }
    }
}
