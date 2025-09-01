using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses.Enums
{
    public enum AppointmentStatus : byte
    {
        Pending = 1,  Confirm = 2, Completed = 3, NotShown = 4,  Cancelled = 5
    }
}
