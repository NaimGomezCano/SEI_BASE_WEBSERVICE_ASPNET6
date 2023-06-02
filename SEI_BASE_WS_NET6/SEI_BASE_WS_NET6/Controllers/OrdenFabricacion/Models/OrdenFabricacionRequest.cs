namespace SEI_WEBSERVICE.Controllers
{
    public class OrdenFabricacionRequest
    {
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string DueDate { get; set; }
        public string IdBestplant { get; set; }
    }
}