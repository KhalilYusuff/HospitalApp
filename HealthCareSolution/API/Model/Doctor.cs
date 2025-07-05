using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.model
{
    public class Doctor : AbstractUser
    {
        public string specialization { get; set; } = "";

       
        public List<Appointment> Appointments { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }




    }
}
