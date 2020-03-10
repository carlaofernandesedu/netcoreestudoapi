using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace prjcontroleversao.Features.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/Product")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value 2.0 " + id.ToString() ;
        }
    }
}