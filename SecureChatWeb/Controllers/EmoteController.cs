using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureChatWeb.Data;
using Microsoft.Extensions.Configuration;

namespace SecureChatWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmoteController : ControllerBase
    {
        public EmoteController(IConfiguration iconfiguration)
        {
            _config = iconfiguration;
        }

        private IConfiguration _config;

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Emoji.Emoji>> Get()
        {
            var EDM = new EmojiDataManager(_config);
            return EDM.GetAllEmoji();
        }

    }
}