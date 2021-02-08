using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoctorAppointment.DatabaseContexts;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("api/Appointment/GetUserProfile")]
        public User GetUserProfile()
        {
            string userid = User.Claims.First(c => c.Type == "UserId").Value;
            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();
            dbcx.User.First(u => u.UserName == userid);
            return dbcx.User.First(u => u.UserName == userid);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Appointment/BookAppointment")]
        public IActionResult BookAppointments(Appointment appObj)
        {
            string userid = User.Claims.First(c => c.Type == "UserId").Value;
            
            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();
            var obj = dbcx.User.First(u => u.UserName == userid);
            appObj.userId = obj.Id;
            dbcx.Appointment.Add(appObj);
            int count = dbcx.SaveChanges();
            return Ok(new { count });
        }

        [HttpPost]
        [Authorize]
        [Route("api/Appointment/UpdateAppointment/{aid:int}")]
        public IActionResult UpdateAppointment([FromRoute] int aid, Appointment appObj)
        {
            //string userid = User.Claims.First(c => c.Type == "UserId").Value;

            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();
            //var obj = dbcx.User.First(u => u. == userid);
            //appObj.userId = obj.Id;
            var v = dbcx.Appointment.First(a=>a.Aid == aid);
            v.doa = appObj.doa;
            v.doctor = appObj.doctor;
            v.hospital = appObj.hospital;
            v.state = appObj.state;
            v.time = appObj.time;
            int count = dbcx.SaveChanges();
            return Ok(new { count });
        }

        [HttpGet]
        [Authorize]
        [Route("api/Appointment/GetAppointment/{aid:int}")]
        public IActionResult GetAppointment([FromRoute] int aid)
        {
            //string userid = User.Claims.First(c => c.Type == "UserId").Value;

            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();
            //var obj = dbcx.User.First(u => u. == userid);
            //appObj.userId = obj.Id;
            var v = dbcx.Appointment.First(a => a.Aid == aid);
            return Ok(v);
        }


        [HttpGet]
        [Authorize]
        [Route("api/Appointment/GetAppointments")]
        public IActionResult GetAppointments()
        {
            string userid = User.Claims.First(c => c.Type == "UserId").Value;

            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();
            var obj = dbcx.User.First(u => u.UserName == userid);
            var obj1 = dbcx.Appointment.Where(a => a.userId == obj.Id).ToList();
            var obj2 = (from us in dbcx.User
                        join
   ap in dbcx.Appointment on us.Id equals ap.userId
                        where us.Id == obj.Id
                        select new Appointment
                        {
                            Aid = ap.Aid,
                            doa = ap.doa,
                            doctor = GetDoctorName(ap.doctor),
                            hospital = GetHospitalName(ap.hospital),
                            state = ap.state == "A" ? "Active" : "Cancelled",
                            time = ap.time,
                            userId = ap.userId
                        }).ToList();
            return Ok(obj2);
        }

        private static string GetHospitalName(string hospital)
        {
            DoctorAppointmentContext dbcx1 = new DoctorAppointmentContext();
            var hospi = dbcx1.Hospital.FirstOrDefault(hos => hos.id == Convert.ToInt32(hospital));
            return hospi.hospital;
        }

        private static string GetDoctorName(string doctor)
        {
            DoctorAppointmentContext dbcx2 = new DoctorAppointmentContext();
            var doci = dbcx2.Doctor.FirstOrDefault(hos => hos.did == Convert.ToInt32(doctor));
            return doci.name;
        }

        [HttpGet]
        [Authorize]
        [Route("api/Appointment/GetHospitals")]
        public IActionResult GetHospitals()
        {
            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();

            return Ok(dbcx.Hospital.ToList());
        }

        [HttpGet]
        [Authorize]
        [Route("api/Appointment/GetDoctors/{id:int}")]
        public IActionResult GetDoctors([FromRoute]int id)
        {
            DoctorAppointmentContext dbcx = new DoctorAppointmentContext();

            return Ok(dbcx.Doctor.Where(d => d.hid == id).ToList());
        }
    }
}
