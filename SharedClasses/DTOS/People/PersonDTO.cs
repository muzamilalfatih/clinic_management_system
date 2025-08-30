namespace SharedClasses
{
    public class PersonDTO
     {
        public int id { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string thirdName { get; set; }
        public string lastName { get; set; }
        public enGender gender { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public PersonDTO(int id, string firstName, string secondName, string thirdName, string lastName, enGender gender, string phone, string address)
         {
             this.id = id;
             this.firstName = firstName;
             this.secondName = secondName;
             this.thirdName = thirdName;
             this.lastName = lastName;
             this.gender = gender;
             this.phone = phone;
             this.address = address;
         }
     }
}
