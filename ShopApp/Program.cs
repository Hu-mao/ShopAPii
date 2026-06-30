
using Microsoft.OpenApi;
using ShopApp.Middlewares;
using System.Reflection;

namespace ShopApp
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimerMiddleware>();
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shop API",
                    Version = "v1",
                    Description = "API для керування товарами магазину."

                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
            });
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseMiddleware<UserCheckMiddleware>();
            app.MapControllers();
            app.UseRequestTimer();

            app.UseStaticFiles();
            app.Run();
        }
    }
}

