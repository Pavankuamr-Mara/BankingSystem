using Infrastructure.Dtos;
using Infrastructure.Exceptions;
using System.Net;

namespace WebAPI.Exceptions
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(RecordNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                RecordNotFoundException => (int)HttpStatusCode.NotFound,
                DeceedingBalanceLimitException => (int)HttpStatusCode.ExpectationFailed,
                ExceedingDepositLimitException => (int)HttpStatusCode.ExpectationFailed,
                ExceedingWithdrawalLimitException => (int)HttpStatusCode.ExpectationFailed,
                ZeroOrNegativeAmountException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}
