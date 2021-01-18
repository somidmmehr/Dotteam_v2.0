using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dotteam.Controllers
{
    public class Login : Controller
    {
        public IActionResult Index()
        {
            return Redirect("Identity/Account/Login");
        }
    }
}
