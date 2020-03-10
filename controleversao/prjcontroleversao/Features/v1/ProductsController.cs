using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace prjcontroleversao.Features.v1
{
    [ApiController]
    [ApiVersion("1.0")]
     [Route("api/Product")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value 1.0 " + id.ToString();
        }
    }
}