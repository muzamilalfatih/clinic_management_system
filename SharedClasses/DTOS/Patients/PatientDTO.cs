namespace SharedClasses
{
    public class PatientDTO
     {
        public int id { get; set; }
        public int userId { get; set; }
        public string medicalHistroy { get; set; }
        public PatientDTO(int id, int userId, string medicalHistroy)
         {
             this.id = id;
             this.userId = userId;
             this.medicalHistroy = medicalHistroy;
         }
     }
}
