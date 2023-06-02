namespace SEI_WEBSERVICE.Controllers
{
    public class SolicitudCompraRequest
    {
        public string ItemCode { get; set; }
        public double Quantity   { get; set; }
        public string LineVendor  { get; set; }
        public string PQTReqDate { get; set; }
        public int OFRelated { get; set; }
        public string FatherCode  { get; set; }
        public string CONRelated  { get; set; }
        public string IdLine_Bestplant { get; set; }
    }
}