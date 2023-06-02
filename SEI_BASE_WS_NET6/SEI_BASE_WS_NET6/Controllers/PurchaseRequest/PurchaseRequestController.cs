using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using RestSharp;
using System.Globalization;

namespace SEI_WEBSERVICE.Controllers
{
    [ApiController]
    [Route("v1")]
    public class PurchaseRequestController : ControllerBase
    {
        private readonly ILogger<OrdenFabricacionController> logger;
        private ServiceLayer serviceLayer;
        private SQL sql;

        public PurchaseRequestController(ILogger<OrdenFabricacionController> _logger)
        {
            logger = _logger;
            serviceLayer = new ServiceLayer(_logger);
            sql = new SQL(_logger);

            serviceLayer.doLogin();
            //sql.openSQLConnection();
        }

        [HttpPost("PurchaseRequest")]
        public async Task<ActionResult<SolicitudCompraResponse>> PostAsync([FromBody] List<SolicitudCompraRequest> requestBody)
        {
            SAPB1.Document document = new SAPB1.Document();
            SAPB1.DocumentLine docLine;


            foreach (var line in requestBody)
            {
                docLine = new SAPB1.DocumentLine();

                //TODO: enviar todos los campos que vienen del body

                docLine.ItemCode = line.ItemCode;
                docLine.Quantity = line.Quantity;
                docLine.LineVendor = line.LineVendor;
                docLine.RequiredDate = DateTime.ParseExact(line.PQTReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //docLine.OFRelated = line.OFRelated;
                //docLine.FatherCode = line.FatherCode;
                //docLine.CONRelated = line.CONRelated;
                //docLine.IdLine_Bestplant = Int32.Parse(line.IdLine_Bestplant);

                document.DocumentLines.Add(docLine);
                docLine = null;
            }


            string body = JsonConvert.SerializeObject(document, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            IRestResponse response = serviceLayer.post("PurchaseRequests", body);

            var response2 = new SolicitudCompraResponse();

            if (!response.IsSuccessful)
            {
                response2.success = false;
                response2.message = response.ErrorMessage;
                throw new Exception("Ha ocurrido un error: " + response.ErrorMessage + "  " + response2.message);
            }
            else
            {
                document = JsonConvert.DeserializeObject<SAPB1.Document>(response.Content);

                response2.success = true;
                response2.message = "Solicitud de compra creado correctamente";
                response2.DocEntry = document.DocEntry;
                response2.DocNum = (int)document.DocNum;
                response2.DocumentLine = new List<DocumentLine>();

                foreach (var item in document.DocumentLines)
                {
                    var line = new DocumentLine();
                    line.LineNum = (int)item.LineNum;
                    line.IdLine_Bestplant = item.U_SEILineOF.ToString();
                    response2.DocumentLine.Add(line);
                    line = null;
                }

            }

            return response2;
        }
    }
}