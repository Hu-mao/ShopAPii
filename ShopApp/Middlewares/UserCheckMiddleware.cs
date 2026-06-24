using Shop.Domain.Models;
using System.Text;
using System.Text.Json;

namespace ShopApp.Middlewares
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public UserCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST" &&
                context.Request.Path == "/api/user/register")
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    leaveOpen: true);

                var body = await reader.ReadToEndAsync();

                context.Request.Body.Position = 0;

                var user = JsonSerializer.Deserialize<User>(
                    body,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (user == null ||
                    user.Id != 1 ||
                    user.Login != "admin")
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(new
                        {
                            message = "No authorization"
                        }));

                    return;
                }
            }

            await _next(context);
        }
    }
}
