using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabDevices
{
    public class UpdateLabDeviceDTO
    {
        public UpdateLabDeviceDTO(int id, string name, string model, string connectionType, string isActive)
        {
            Id = id;
            Name = name;
            Model = model;
            ConnectionType = connectionType;
            IsActive = isActive;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string ConnectionType { get; set; }
        public string IsActive { get; set; }
    }
}
