using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorAppointment.Models
{
    public class Doctor
    {
        public int hid { get; set; }
        [Key]
        public int did { get; set; }
        public string name { get; set; }
    }
}
