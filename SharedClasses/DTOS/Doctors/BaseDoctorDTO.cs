using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS
{
    public class BaseDoctorDTO
    {
       
        public BaseDoctorDTO(int specializationId, byte prevExperienceYears, DateTime joinDate,
            string bio, decimal consultationFee)
        {
            this.specializationId = specializationId;
            this.prevExperienceYears = this.prevExperienceYears;
            this.joinDate = joinDate;
            this.bio = bio;
            this.consultationFee = consultationFee;
        }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Invalid id")]
        public int specializationId { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="Must be 1 or more")]
        public byte prevExperienceYears { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime joinDate { get; set; }
        [Required]
        [MinLength(10,ErrorMessage ="At least 10 character")]
        public string bio { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="The fee must 1 or more")]
        public decimal consultationFee { get; set; }
    }
}
