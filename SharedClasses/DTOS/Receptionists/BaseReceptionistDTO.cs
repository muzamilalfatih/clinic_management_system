using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class BaseReceptionistDTO
    {
        public BaseReceptionistDTO(int shiftTypeId, DateTime hireDate)
        {
            ShiftTypeId = shiftTypeId;
            HireDate = hireDate;
        }
        [Required(ErrorMessage ="This field is required!")]
        [Range(1, int.MaxValue,ErrorMessage ="Invalid id!")]
        public int ShiftTypeId { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public DateTime HireDate { get; set; }
    }
}
