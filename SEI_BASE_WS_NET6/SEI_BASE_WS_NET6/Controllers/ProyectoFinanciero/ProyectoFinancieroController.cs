using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace SEI_WEBSERVICE.Controllers
{

    //[ApiController]
    //[Route("v1")]
    public class ProyectoFinancieroController : ControllerBase
    {
        private readonly ILogger<ProyectoFinancieroController> logger;
        private SQL sql;

        public ProyectoFinancieroController(ILogger<ProyectoFinancieroController> _logger)
        {
            logger = _logger;
            sql = new SQL(_logger);
            sql.openSQLConnection();
        }

        //[HttpGet("Proyectos/{CON}")]
        public async Task<ActionResult<string>> GetActionAsync([FromRoute] string con)
        {
            ProyectoFinancieroResponse oPF = null;
            List<ProyectoFinancieroResponse> oList = new List<ProyectoFinancieroResponse>();

            string SSQL = "SELECT T0.Project as CON, T0.CardCode, T0.CardName, format(T0.DocDueDate, 'yyyy-MM-dd') as ShipDate, T0.Comments, T0.U_SEIAplCON, T0.U_SEIObsCON, T0.U_SEIEmbalCON, T0.U_SEIComOTCON ";
            SSQL += "FROM ORDR T0 ";
            SSQL += "WHERE T0.Project = '" + con + "'";

            using (SqlCommand command = new SqlCommand(SSQL, sql.sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oPF = new ProyectoFinancieroResponse();

                        oPF.CON = reader.GetString("CON");
                        oPF.CardCode = reader.GetString("CardCode");
                        oPF.CardName = reader.GetString("CardName");
                        oPF.ShipDate = reader.GetString("ShipDate");
                        oPF.Comments = reader.IsDBNull("Comments") ? null : reader.GetString("Comments");
                        oPF.U_SEIAplCON = reader.GetString("U_SEIAplCON");
                        oPF.U_SEIObsCON = reader.GetString("U_SEIObsCON");
                        oPF.U_SEIEmbalCON = reader.GetString("U_SEIEmbalCON");
                        oPF.U_SEIComOTCON = reader.GetString("U_SEIComOTCON");

                        oList.Add(oPF);
                        oPF = null;
                    }
                }
            }

            string proyectoFinancieroReturn = JsonConvert.SerializeObject(oList, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });


            return proyectoFinancieroReturn;
        }
    }
}

