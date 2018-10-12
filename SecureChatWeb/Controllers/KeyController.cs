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
        
        public KeyController(IKeyHandler KeyHandles)
        {
            _KeyHandles = KeyHandles;
        }

        private IKeyHandler _KeyHandles;

        [HttpGet]
        public string Get()
        {
            return string.Empty;
        }

        [HttpGet("GetAllKeys/{Room}", Name = "Get")]
        public IEnumerable<string> Get(string Room)
        {
            return _KeyHandles.GetPublicKeys(Room);
        }

        //[Route("Keys/{Room:string}")]
        //public IEnumerable<string> GetAllKeys(string Room)
        //{
        //    return _KeyHandles.GetPublicKeys(Room);
        //}

        [HttpGet("AddKey/{Room}/{PublicKey}")]
        public string AddKey(string Room, string PublicKey)
        {
            _KeyHandles.AddKey(Room, PublicKey);
            return "add";
        }


        [HttpGet("DeleteKey/{Room}/{PublicKey}")]
        public string DeleteKey(string Room, string PublicKey)
        {
            _KeyHandles.RemoveKey(Room, PublicKey);
            return "delete";
        }


        

    }
}