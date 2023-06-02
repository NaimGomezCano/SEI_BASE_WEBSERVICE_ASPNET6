using Microsoft.OpenApi.Models;
using SEI_WEBSERVICE.Middlewares;

namespace SEI_WEBSERVICE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;
            IWebHostEnvironment environment = builder.Environment;
            AppSettings.loadSettings(configuration);

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GALAN_WEBSERVICE_BESTPLAN", Version = "v1" });

            });

            builder.Logging.AddLog4Net("log4net.config");

            var app = builder.Build();


            app.UseMiddleware<ExceptionManagerMiddleware>();
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/v1"), appBuilder =>
            {
                appBuilder.UseMiddleware<AuthenticationMiddleware>();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}