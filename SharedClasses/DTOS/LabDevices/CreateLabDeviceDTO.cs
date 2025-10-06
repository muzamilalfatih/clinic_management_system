using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.LabDevices
{
    public class CreateLabDeviceDTO
    {
        public CreateLabDeviceDTO(string name, string model, string connectionType, bool isActive)
        {
            Name = name;
            Model = model;
            ConnectionType = connectionType;
            IsActive = isActive;
        }

        public string Name { get; set; }
        public string Model { get; set; }
        public string ConnectionType { get; set; }
        public bool IsActive { get; set; }
    }
}
