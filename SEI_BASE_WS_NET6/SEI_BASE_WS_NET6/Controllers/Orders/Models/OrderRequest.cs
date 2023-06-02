using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SEI_WEBSERVICE
{
    public class OrderRequest
    {
        public string CardCode { get; set; }
        public string U_SEIIDSF { get; set; }
        public double? DiscountPercent { get; set; }
        public DateTimeOffset postingDate { get; set; }
        public DateTimeOffset deliveryDate { get; set; }
        public DateTimeOffset documentDate { get; set; }

        public List<OrderLineRequest> OrderLines { get; set; }

        public List<DocumentRequest>? Documents { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
