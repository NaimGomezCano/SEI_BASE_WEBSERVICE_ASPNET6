using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SEI_WEBSERVICE.Controllers
{
    [ApiController]
    [Route("v1")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> logger;
        private SQL sql;

        public StockController(ILogger<StockController> _logger)
        {
            logger = _logger;
            sql = new SQL(_logger);
            sql.openSQLConnection();
        }

        [HttpGet("Stock")]
        public async Task<ActionResult<List<StockResponse>>> GetActionAsync()
        {
            StockResponse oSR = null;
            List<StockResponse> oList = new List<StockResponse>();
            string SSQL = "SELECT T0.ItemCode, sum (T1.OnHand) as OnHand, sum(T1.IsCommited) as IsCommited, sum(T1.OnOrder) as OnOrder, T0.MinLevel, T0.MaxLevel ";
            SSQL += "FROM OITM T0 ";
            SSQL += "INNER JOIN OITW T1 on T0.ItemCode = T1.ItemCode ";
            SSQL += "INNER JOIN OWHS T2 on T1.WhsCode = T2.WhsCode ";
            SSQL += "WHERE COALESCE(T2.U_SEI_BESTPLAN, 'N') = 'Y' ";
            SSQL += "GROUP BY T0.ItemCode, T0.MinLevel, T0.MaxLevel ";
            SSQL += "HAVING sum (T1.OnHand) <> 0 OR sum(T1.IsCommited) <> 0 OR sum(T1.OnOrder) <> 0";

            using (SqlCommand command = new SqlCommand(SSQL, sql.sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oSR = new StockResponse();
                        oSR.ItemCode = reader.GetString("ItemCode");
                        oSR.OnHand = double.Parse(reader.GetDecimal("OnHand").ToString());
                        oSR.IsCommited = double.Parse(reader.GetDecimal("OnHand").ToString());
                        oSR.OnOrder = double.Parse(reader.GetDecimal("OnOrder").ToString());
                        oSR.MinLevel = double.Parse(reader.GetDecimal("MinLevel").ToString());
                        oSR.MaxLevel = double.Parse(reader.GetDecimal("MaxLevel").ToString());

                        oList.Add(oSR);
                        oSR = null;
                    }
                }
            }

            string stockReturn = JsonConvert.SerializeObject(oList, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });


            return oList;
        }
    }
}

