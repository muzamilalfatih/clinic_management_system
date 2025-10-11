using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.PescriptionItem
{
    public class AddNewPrescriptionItemDTO
    {
        public AddNewPrescriptionItemDTO(string medicineName, string dosage, string frequency, string duration)
        {
            MedicineName = medicineName;
            Dosage = dosage;
            Frequency = frequency;
            Duration = duration;
        }

        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
    }
}
