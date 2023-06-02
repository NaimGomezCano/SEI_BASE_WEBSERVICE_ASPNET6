using SEI_WEBSERVICE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using SEI_WEBSERVICE;
using SEI_WEBSERVICE.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SEI_WEBSERVICE;

class ServiceLayer
{
    public string b1session = null;
    private readonly ILogger logger;
    public class Login
    {
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }

    public ServiceLayer(ILogger _logger)
    {
        logger = _logger;
    }

    ~ServiceLayer()
    {
        logger.LogInformation("Logout ServiceLayer");
        post("Logout", "");
        logger.LogInformation("Logout ServiceLayer correcto");
    }

    public void doLogin()
    {
        logger.LogInformation("Login ServiceLayer");

        var login = new Login();
        login.CompanyDB = AppSettings.companyDB;
        login.UserName = AppSettings.userName;
        login.Password = AppSettings.password;

        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        var body = JsonConvert.SerializeObject(login);

        var response = post("Login", body);

        if(!response.IsSuccessful)
        {
            throw new Exception("No se ha podido hacer login a ServiceLayer | StatusCode: " + response.StatusCode + " Content: " + response.Content);
        }

        var content = response.Content;
        var data = (JObject)JsonConvert.DeserializeObject(content);
        b1session = data["SessionId"].ToString();

        logger.LogInformation("Login ServiceLayer correcto");
    }

    public IRestResponse post(string route, string body)
    {
        var client = new RestClient(AppSettings.uri + "/" + route);
        var request = new RestRequest();

        request.Method = Method.POST;
        request.AddHeader("Accept", "application/json");
        request.AddParameter("application/json", body, ParameterType.RequestBody);
        request.AddJsonBody(body);
        request.AddCookie("B1SESSION", b1session);

        var response = client.Execute(request);

        return response;
    }


    public IRestResponse getById(string route, string id)
    {

        return get(route + "("+id+")");
    }

    public IRestResponse get(string route)
    {
        var client = new RestClient(AppSettings.uri + "/" + route);
        var request = new RestRequest();

        request.Method = Method.GET;
        request.AddHeader("Accept", "application/json");
        request.AddCookie("B1SESSION", b1session);

        var response = client.Execute(request);

        return response;
    }
}