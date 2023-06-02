using SEI_WEBSERVICE;
using Newtonsoft.Json;

namespace SEI_WEBSERVICE
{
    public class OrderResponse : OrderRequest
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}