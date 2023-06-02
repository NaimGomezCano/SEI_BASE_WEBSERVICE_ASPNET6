namespace SEI_WEBSERVICE.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<AuthenticationMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Accediendo a Login");
            if (AppSettings.authToken != context.Request.Headers["token"].FirstOrDefault())
            {
                _logger.LogError("Credenciales incorrectas " + context.Request.Headers["user"].FirstOrDefault() + " " + context.Request.Headers["pwd"].FirstOrDefault());
                throw new UnauthorizedAccessException("Token does not match");
            }
            _logger.LogInformation("Login correcto");

            await _next(context);
        }
    }
}
