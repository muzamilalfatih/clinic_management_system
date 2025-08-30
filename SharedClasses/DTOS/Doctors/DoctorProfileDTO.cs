using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Doctors
{
    public class DoctorProfileDTO
    {
        public DoctorProfileDTO(string specialization, byte prevExperienceYears, DateTime joinDate, string bio, decimal consulationFee)
        {
            this.specialization = specialization;
            this._prevExperienceYears = prevExperienceYears;
            this._joinDate = joinDate;
            this.bio = bio;
            this.consulationFee = consulationFee;
        }

        public string specialization {  get; set; }
        public int experienceYears => _prevExperienceYears + ((DateTime.Now - _joinDate).Days / 350);
        private int _prevExperienceYears { get; set; }
        private DateTime _joinDate { get; set; }
        public string bio {  get; set; }
        public decimal consulationFee { get; set; }
    }
}
