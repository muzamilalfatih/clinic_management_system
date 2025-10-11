using SharedClasses.DTOS.PescriptionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.DTOS.Prescription
{
    public class AddNewPrescriptionRequestDTO
    {
        public AddNewPrescriptionRequestDTO(AddNewPrescriptionDTO prescriptionDTO, List<AddNewPrescriptionItemDTO> items)
        {
            PrescriptionDTO = prescriptionDTO;
            this.items = items;
        }

        public AddNewPrescriptionDTO PrescriptionDTO { get; set; }
        public List<AddNewPrescriptionItemDTO> items { get; set; }
    }
}
