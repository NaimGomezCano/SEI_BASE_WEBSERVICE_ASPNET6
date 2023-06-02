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
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> logger;
        private ServiceLayer serviceLayer;
        private SQL sql;

        public OrdersController(ILogger<OrdersController> _logger)
        {
            logger = _logger;
            serviceLayer = new ServiceLayer(_logger);
            sql = new SQL(_logger);

            serviceLayer.doLogin();
            sql.openSQLConnection();
        }

        //[HttpPost("orders")]
        public async Task<ActionResult<OrderResponse>> PostAsync([FromBody] OrderRequest sfOrder)
        {
            OrderResponse orderResponse = new OrderResponse();
            String docEntry = "";
            String docNum = "";

            logger.LogInformation("Accediendo a /Orders - U_SEIIDSF:" + sfOrder.U_SEIIDSF);
            logger.LogInformation("Json body: " + sfOrder.ToString());

            return orderResponse;
        }

        //[HttpGet("orders")]
        public async Task<ActionResult<string>> GetActionAsync()
        {
            IRestResponse response = serviceLayer.get("Orders(13)");
            //IRestResponse response = serviceLayer.getById("Orders", "13");

            if(!response.IsSuccessful)
            {
                throw new Exception("Ha ocurrido un error");
            }

            SAPB1.Document order = JsonConvert.DeserializeObject<SAPB1.Document>(response.Content);

            string orderReturn = JsonConvert.SerializeObject(order, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            using (SqlCommand command = new SqlCommand("SELECT CardCode FROM ORDR", sql.sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cardCode = reader.GetString(0);
                        Console.WriteLine($"CardCode: {cardCode}");
                    }
                }
            }

            //var sSQL = "SELECT T0.""AbsEntry"", T1.""PickEntry"", T1.""PickStatus"", T2.""Quantity"", T2.""WhsCode""";
            //var oDataSet = new DataSet();
            //var oSqlDataAdapter = new SqlDataAdapter(sSQL, sql.sqlConnection);
            //oSqlDataAdapter.Fill(oDataSet);

            //if(oDataSet.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow RowDet in oDataSet.Tables[0].Rows)
            //    {
            //        string cardCode = RowDet.
            //        Console.Write($"{element} ");
            //    }
            //}

            return orderReturn;
        }
    }
}