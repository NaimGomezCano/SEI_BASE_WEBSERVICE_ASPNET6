using System;

namespace SEI_WEBSERVICE
{
    public class ComprometidoResponse
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string ObjType { get; set; }
        public string ItemCode { get; set; }
        public string ShipDate { get; set; } //Formato yyyy-mm-dd
        public double OpenQty { get; set; }
    }

}

