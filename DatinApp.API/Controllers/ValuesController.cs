using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DatinApp.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DatinApp.API.Controllers
{

    //http:localhost:500/api/values
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext contexts;

        public ValuesController(DataContext contexts)
        {
            this.contexts = contexts;

        }

        // Get api/values
        [HttpGet]
        public  async Task<IActionResult> GetValues()
        {

            var values =  await contexts.tbl_values.ToListAsync();

            return Ok(values);
        }
        // Get api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var values = contexts.tbl_values.FirstOrDefault(x => x.Id == id);
            return Ok(values);
        }

        public void Post([FromBody] string value)
        {


        }


        [HttpPut("id")]
        public void Put(int id, [FromBody] string value)
        {


        }
        [HttpDelete("id")]
        public void Delete(int id)
        {


        }
    }
}