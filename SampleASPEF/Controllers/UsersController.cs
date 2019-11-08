using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SampleASPEF.Controllers
{
    public class UsersController : Controller
    {
        private UserManager<IdentityUser> _user;
        public UsersController(UserManager<IdentityUser> user)
        {
            _user = user;
        }

        public IActionResult Index()
        {
            var user = User.Identity.Name;
            return Content($"Username anda: {user}");
        }

        public async Task<IActionResult> Register()
        {
            var newUser = new IdentityUser
            {
                UserName = "erickkurniawan",
                Email = "erickkur@gmail.com"
            };

            var result = await _user.CreateAsync(newUser, "Rahasia-2019");
            if (result.Succeeded)
            {
                return Content("Registrasi berhasil");
            }
            else
            {
                return Content("Registrasi gagal !");
            }

        }
    }
}