using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.PescriptionItem
{
    public class PrescrriptionItemDTO
    {
        public PrescrriptionItemDTO(int id, int prescriptionId, string medicineName, string dosage, string frequency, string duration)
        {
            Id = id;
            PrescriptionId = prescriptionId;
            MedicineName = medicineName;
            Dosage = dosage;
            Frequency = frequency;
            Duration = duration;
        }

        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
    }
}
