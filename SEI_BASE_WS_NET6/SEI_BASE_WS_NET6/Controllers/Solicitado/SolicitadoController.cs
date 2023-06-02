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

    //[ApiController]
    //[Route("v1")]
    public class SolicitadoController : ControllerBase
    {
        private readonly ILogger<SolicitadoController> logger;
        private SQL sql;

        public SolicitadoController(ILogger<SolicitadoController> _logger)
        {
            logger = _logger;
            sql = new SQL(_logger);
            sql.openSQLConnection();
        }

        //[HttpGet("Solicitado")]
        public async Task<ActionResult<string>> GetActionAsync()
        {
            SolicitadoResponse oSolR = null;
            List<SolicitadoResponse> oList = new List<SolicitadoResponse>();
           
            string SSQL = "SELECT T0.DocEntry, T1.DocNum, T0.ObjType,T0.ItemCode, format(T0.ShipDate, 'yyyy-MM-dd') AS ShipDate, T0.OpenQty ";
            SSQL += "FROM POR1 T0 ";
            SSQL += "INNER JOIN OPOR T1 on T0.DocEntry = T1.DocEntry ";
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
                        oSolR = new SolicitadoResponse();
                        oSolR.DocEntry = reader.GetInt32("DocEntry");
                        oSolR.DocNum = reader.GetInt32("DocNum");
                        oSolR.ObjType = reader.GetString("ObjType");
                        oSolR.ItemCode = reader.GetString("ItemCode");
                        oSolR.ShipDate = reader.GetString("ShipDate");
                        oSolR.OpenQty = double.Parse(reader.GetDecimal("OpenQty").ToString());

                        oList.Add(oSolR);
                        oSolR = null;
                    }
                }
            }

            string solicitadoReturn = JsonConvert.SerializeObject(oList, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });


            return solicitadoReturn;
        }
    }
}
