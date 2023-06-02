using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace SEI_WEBSERVICE.Controllers
{
    [ApiController]
    [Route("v1")]
    public class EscandalloController : ControllerBase
    {
        private readonly ILogger<EscandalloController> logger;
        private SQL sql;

        public EscandalloController(ILogger<EscandalloController> _logger)
        {
            logger = _logger;
            sql = new SQL(_logger);

            sql.openSQLConnection();
        }

        [HttpGet("Escandallo/{con}")]
        public async Task<ActionResult<List<EscandalloResponse>>> GetActionAsync([FromRoute] string con)
        {
            EscandalloResponse oEsc = null;
            List<EscandalloResponse> oList = new List<EscandalloResponse>();

            string SSQL = "WITH CTEestrutura (Nivel, Father,NomPare, Code, NomComponent, Qauntity, FaseMuntatge) ";
            SSQL += "AS ";
            SSQL += "(SELECT 0 AS Level, T0.Father, T3.ItemName, T0.Code, T2.ItemName, T1.Qauntity, T0.U_FaseMuntatge ";
            SSQL += "FROM ITT1 T0 ";
            SSQL += "INNER JOIN OITT T1 ON T1.Code = T0.Father ";
            SSQL += "INNER JOIN OITM T2 ON T2.Itemcode = T0.Code ";
            SSQL += "INNER JOIN OITM T3 ON T3.Itemcode = T0.Father ";
            SSQL += "WHERE T0.Father in (SELECT R1.ItemCode FROM ORDR R0 INNER JOIN RDR1 R1 ON R0.DocEntry =  R1.DocEntry AND ";
            SSQL += "R0.Project = '" + con + "' AND R0.U_SEITipo = 'Expedición') ";
            SSQL += "UNION ALL ";
            SSQL += "SELECT Nivel + 1, T0.Father, T4.ItemName, T0.Code, T2.ItemName, T3.Qauntity, T0.U_FaseMuntatge ";
            SSQL += "FROM ITT1 T0 ";
            SSQL += "INNER JOIN CTEestrutura as B0 on B0.Code = T0.Father ";
            SSQL += "INNER JOIN OITM T2 ON T2.Itemcode = T0.Code ";
            SSQL += "INNER JOIN OITT T3 ON T3.Code = T0.Father ";
            SSQL += "INNER JOIN OITM T4 ON T4.Itemcode = T0.Father) ";
            SSQL += "SELECT ";
            SSQL += "   B0.Nivel, " +
                "       COALESCE(B0.Father, '') AS Father, " +
                "       COALESCE(B0.NomPare, '') AS NomPare, " +
                "       COALESCE(B0.Code, '') AS Code, " +
                "       COALESCE(B0.NomComponent, '') AS NomComponent, " +
                "       COALESCE(B0.Qauntity, 0) AS Quantity, " +
                "       COALESCE(B0.FaseMuntatge, '') AS FaseMuntatge , " +
                "       COALESCE(TM.InvntryUom, '') AS InvntryUom, " +
                "       COALESCE(TM.CardCode, '') AS CardCode, " +
                "       COALESCE(TM.PrcrmntMtd, '') AS PrcrmntMtd," +
                "       COALESCE(TM.LeadTime, 0) LeadTime ";
            SSQL += "FROM CTEestrutura B0 ";
            SSQL += "inner join OITM TM ON B0.Code = TM.ItemCode";

            using (SqlCommand command = new SqlCommand(SSQL, sql.sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oEsc = new EscandalloResponse();
                        oEsc.Nivel = reader.GetInt32("Nivel");
                        oEsc.ItemCode = reader.GetString("Code");
                        oEsc.FatherCode = reader.GetString("Father");
                        oEsc.ItemName = reader.GetString("NomComponent");
                        oEsc.FaseMontaje = reader.GetString("FaseMuntatge");
                        oEsc.Quantity = double.Parse(reader.GetDecimal("Quantity").ToString());
                        oEsc.TipoUnidad = reader.GetString("InvntryUom");
                        oEsc.CardCode = reader.GetString("CardCode");
                        oEsc.PrcrmntMtd = reader.GetString("PrcrmntMtd");
                        oEsc.LeadTime = reader.GetInt32("LeadTime");

                        oList.Add(oEsc);
                        oEsc = null;
                    }
                }
            }

            string escandalloReturn = JsonConvert.SerializeObject(oList, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });


            //return escandalloReturn;
            return oList;
        }
    }
}