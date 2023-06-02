using System.Collections.Generic;

namespace SEI_WEBSERVICE.Controllers
{
    public class SolicitudCompraResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public List<DocumentLine> DocumentLine { get; set; }
    }
}