using System;

namespace SEI_WEBSERVICE
{
    public class StockResponse
    {
        public string ItemCode { get; set; }
        public double OnHand { get; set; }
        public double IsCommited { get; set; }
        public double OnOrder { get; set; }
        public double MinLevel { get; set; }
        public double MaxLevel { get; set; }
    }
}

