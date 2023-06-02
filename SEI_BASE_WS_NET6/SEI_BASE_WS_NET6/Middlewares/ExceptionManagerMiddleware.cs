using Microsoft.OData;
using System.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using SEI_WEBSERVICE.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SEI_WEBSERVICE.Middlewares
{
    public enum ErrorCodes
    {
        BusinessLogic = -2,
        Generic = -3,
        Internal = -4,
    }

    internal class ExceptionManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionManagerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<AuthenticationMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError("ERROR! -> Accediendo a ExceptionManager");

                    ErrorMessage error = new ErrorMessage();
                    context.Response.StatusCode = 400;

                    _logger.LogError("Raw exception: " + JsonConvert.SerializeObject(ex));
                  
                    error.code = ErrorCodes.Generic;
                    if(ex.InnerException != null)
                    {
                        error.message = $"{ex.Message} => {ex.InnerException.Message}";
                    }
                    else
                    {
                        error.message = $"{ex.Message}";
                    }
                    error.trace = ex.StackTrace;

                    _logger.LogError("Return error: " + JsonConvert.SerializeObject(error));
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Ha ocurrido una excepcion en el ExceptionManager!");
                _logger.LogCritical("Raw exception: " + JsonConvert.SerializeObject(ex));

                ErrorMessage error = new ErrorMessage();
                error.code = ErrorCodes.Internal;
                error.message = $"FAILSAFE: {ex.Message}";
                error.trace = ex.StackTrace;

                context.Response.StatusCode = 400;

                _logger.LogCritical("Devolviendo respuesta FAILSAFE");

                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                return;
            }
        }
    }
}