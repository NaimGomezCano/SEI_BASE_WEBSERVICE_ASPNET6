using Newtonsoft.Json;

namespace SEI_WEBSERVICE
{
    public class DocumentRequest
    {
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string Base64 { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}