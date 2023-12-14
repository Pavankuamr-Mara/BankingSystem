using WebAPI.Exceptions;

namespace WebAPI
{
    public static class ConfigureMiddleware
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
