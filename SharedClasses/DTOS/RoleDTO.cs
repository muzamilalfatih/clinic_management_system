namespace SharedClasses
{
    public class RoleDTO
     {
        public int id { get; set; }
        public string name { get; set; }
        public RoleDTO(int id, string name)
         {
             this.id = id;
             this.name = name;
         }
     }
}
