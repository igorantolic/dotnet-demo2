using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ValuesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //var logFilePath = $@"{_hostingEnvironment.ContentRootPath}\logs\log.txt";
            //try
            //{
            //    var fn = $@"{_hostingEnvironment.ContentRootPath}\logs\log.txt";
            //    string v = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} test\r\n";
            //    System.IO.File.AppendAllText(fn, v);
            //    return new string[] { "OK", "...", _hostingEnvironment.ContentRootPath, "OK "+ fn
            //    ,"-----------------", System.IO.File.ReadAllText ($"{logFilePath}")};
            //}
            //catch (Exception ex )
            //{
            //    return new string[] { "ERROR",  _hostingEnvironment.ContentRootPath , ex.Message

            //    };
            //}
            var logFilePath = $@"{_hostingEnvironment.ContentRootPath}\logs\log.txt";
            return new string[] { "ContentRootPath: " + _hostingEnvironment.ContentRootPath, 
                "Log "+ logFilePath
                ,"-----------------", System.IO.File.ReadAllText ($"{logFilePath}")};

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
