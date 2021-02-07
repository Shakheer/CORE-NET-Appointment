using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorAppointment.Models
{
    public class Hospital
    {
        [Key]
        public int id { get; set; }
        public string hospital { get; set; }
    }
}
