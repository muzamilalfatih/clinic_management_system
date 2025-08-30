using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS
{
    public class CreateAuditLogDTO
    {
        public CreateAuditLogDTO(string entityName, int entityId, string action, int performedBy,
                            string? oldValue = null, string? newValue = null)
        {
            this.entityName = entityName;
            this.entityId = entityId;
            this.action = action;
            this.performedBy = performedBy;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public string entityName {  get; set; }
        public int entityId { get; set; }
        public string action { get; set; }
        public int performedBy { get; set; }
        public string? oldValue { get; set; }
        public string? newValue { get; set; }
    }
}
