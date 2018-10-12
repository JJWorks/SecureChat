using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureChatWeb.AsyncKey;

namespace SecureChatWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        /// <summary>
        /// Creates an instance of Emotecontroller.
        /// </summary>
        /// <param name="iconfiguration"></param>
        public KeyController(IKeyHandler KeyHandles)
        {
            _KeyHandles = KeyHandles;
        }

        private IKeyHandler _KeyHandles;


        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<string> Get(string Room)
        {
            _KeyHandles.GetPublicKeys("", "");
            return _KeyHandles.GetPublicKeys(Room);
        }



        [HttpPost]
        public void Post([FromBody] string value)
        {
            _KeyHandles.AddKey("", "", "");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _KeyHandles.RemoveKey("", "");
        }

    }
}