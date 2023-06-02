using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SEI_WEBSERVICE.Controllers
{
    //[ApiController]
    //[Route("v1")]
    public class ComprometidoController : ControllerBase
    {
        private readonly ILogger<ComprometidoController> logger;
        private SQL sql;

        public ComprometidoController(ILogger<ComprometidoController> _logger)
        {
            logger = _logger;
            sql = new SQL(_logger);
            sql.openSQLConnection();
        }

        //[HttpGet("Comprometido")]
        public async Task<ActionResult<string>> GetActionAsync()
        {
            ComprometidoResponse oCR = null;
            List<ComprometidoResponse> oList = new List<ComprometidoResponse>();

            string SSQL = "SELECT T0.DocEntry, T1.DocNum, T0.ObjType,T0.ItemCode, format(T0.ShipDate, 'yyyy-MM-dd') AS ShipDate, T0.OpenQty ";
            SSQL += "FROM RDR1 T0 ";
            SSQL += "INNER JOIN ORDR T1 on T0.DocEntry = T1.DocEntry ";
            SSQL += "WHERE T0.LineStatus = 'O' ";
            SSQL += "union all ";
            SSQL += "SELECT T0.DocEntry, T1.DocNum, T1.ObjType,T0.ItemCode, format(T1.DueDate, 'yyyy-MM-dd') AS ShipDate, (T0.BaseQty - T0.ReleaseQty) as OpenQty ";
            SSQL += "FROM WOR1 T0 ";
            SSQL += "INNER JOIN OWOR T1 on T0.DocEntry = T1.DocEntry ";
            SSQL += "WHERE T0.Status != 'C'";

            using (SqlCommand command = new SqlCommand(SSQL, sql.sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oCR = new ComprometidoResponse();
                        oCR.DocEntry = reader.GetInt32("DocEntry");
                        oCR.DocNum = reader.GetInt32("DocNum");
                        oCR.ObjType = reader.GetString("ObjType");
                        oCR.ItemCode = reader.GetString("ItemCode");
                        oCR.ShipDate = reader.GetString("ShipDate"); //Format en la Query
                        oCR.OpenQty = double.Parse(reader.GetDecimal("OpenQty").ToString());

                        oList.Add(oCR);
                        oCR = null;
                    }
                }
            }

            string comprometidoReturn = JsonConvert.SerializeObject(oList, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });


            return comprometidoReturn;
        }
    }
}
