namespace SharedClasses
{
    public class AuditLogDTO
     {
        public int id { get; set; }
        public string entityName { get; set; }
        public int entityId { get; set; }
        public string action { get; set; }
        public int performedBy { get; set; }
        public DateTime performedAt { get; set; }
        public object? oldValues { get; set; }
        public object? newValues { get; set; }
        public AuditLogDTO(int id, string entityName, int entityId, string action, int performedBy, DateTime performedAt, object? oldValues, object? newValues)
         {
             this.id = id;
             this.entityName = entityName;
             this.entityId = entityId;
             this.action = action;
             this.performedBy = performedBy;
             this.performedAt = performedAt;
             this.oldValues = oldValues;
             this.newValues = newValues;
         }
     }
}
