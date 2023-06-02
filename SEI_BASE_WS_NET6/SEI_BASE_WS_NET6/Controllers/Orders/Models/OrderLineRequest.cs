using Newtonsoft.Json;

namespace SEI_WEBSERVICE
{
    public class OrderLineRequest
    {
        public string U_SEIIDSF { get; set; }
        public string LicensePlate { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public string? TaxCode { get; set; }
        public double? UnitPrice { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}