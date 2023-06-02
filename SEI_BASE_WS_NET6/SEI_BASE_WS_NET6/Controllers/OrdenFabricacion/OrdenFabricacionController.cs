using SEI_WEBSERVICE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using SEI_WEBSERVICE;
using SEI_WEBSERVICE.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SEI_WEBSERVICE;
using System.Data;
using System.Data.SqlClient;

namespace SEI_WEBSERVICE.Controllers
{
    //[ApiController]
    //[Route("v1")]
    public class OrdenFabricacionController : ControllerBase
    {
        private readonly ILogger<OrdenFabricacionController> logger;
        private ServiceLayer serviceLayer;
        private SQL sql;

        public OrdenFabricacionController(ILogger<OrdenFabricacionController> _logger)
        {
            logger = _logger;
            serviceLayer = new ServiceLayer(_logger);
            sql = new SQL(_logger);

            serviceLayer.doLogin();
            sql.openSQLConnection();
        }

        //[HttpPost("OrdenFabricacion")]
        public ActionResult<OrdenFabricacionResponse> Post([FromBody] OrdenFabricacionRequest requestBody)
        {
            var response = new OrdenFabricacionResponse();

            return response;
        }
    }
}