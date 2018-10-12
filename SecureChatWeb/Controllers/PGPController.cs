using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecureChatWeb.AsyncKey;
using System.Net;

namespace SecureChatWeb.Controllers
{
    public class PGPController : Controller
    {
        public PGPController(IKeyHandler _k)
        {
            KeyFun = _k;
        }

        IKeyHandler KeyFun;

        public IActionResult Index()
        {
            var RandomString = WebUtility.UrlEncode(Guid.NewGuid().ToString());
            return Redirect("/PGP/chat/" + RandomString);
        }

        [Route("PGP/chat/{RoomNumber}")]
        public IActionResult chat(string RoomNumber)
        {
            KeyFun.AddRoom(RoomNumber);
            ViewData["RoomNumber"] = RoomNumber;
            return View();
        }

    }
}