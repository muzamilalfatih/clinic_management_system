namespace SharedClasses
{
    public class DoctorDTO
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int specializationId { get; set; }
        private byte prevExperienceYears { get; set; }
        private DateTime joinDate { get; set; }
        public int experienceYears => prevExperienceYears + ((DateTime.Now - joinDate).Days / 350);
        public string bio { get; set; }
        public decimal consulationFee { get; set; }
        public DoctorDTO(int id, int userId, int specializationId, byte prevExperienceYears, DateTime joinDate, string bio, decimal consulationFee)
        {
            this.id = id;
            this.userId = userId;
            this.specializationId = specializationId;
            this.prevExperienceYears = prevExperienceYears;
            this.joinDate = joinDate;
            this.bio = bio;
            this.consulationFee = consulationFee;
        }
    }
}
