using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleASPEF.Models;

namespace SampleASPEF.Controllers
{
    //penggunaan session
    // HttpContext.Session.SetString("username",pgn.Username);
    //HttpContext.Session.SetString("aturan",pgn.Aturan);
    // if (HttpContext.Session.GetString("aturan") != null &&
    //HttpContext.Session.GetString("aturan") == aturan)
    // HttpContext.Session.Clear();
    public class CekSessionController : Controller
    {
        public IActionResult Index()
        {
            var student = new Student
            {
                FirstMidName = "Erick",
                Address = "Jl Jambu 12",
                LastName = "Kurniawan"
            };

            var serialisasiStudent = JsonConvert.SerializeObject(student);
            HttpContext.Session.SetString("username",serialisasiStudent);
            return Content("Session berhasil dibuat....");
        }

        public IActionResult CekSession()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var data = HttpContext.Session.GetString("username");
                var student = JsonConvert.DeserializeObject<Student>(data);
                return Content($"{student.FirstMidName} {student.LastName} {student.Address}");
            }
            else
            {
                return Content("Session tidak ditemukan");
            }
        }
    }
}