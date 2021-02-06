using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorAppointment.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Aid { get; set; }
        public int userId { get; set; }
        public string hospital { get; set; }
        public string doctor { get; set; }
        public DateTime doa { get; set; }
        public string time { get; set; }
        public string state { get; set; }
    }
}
