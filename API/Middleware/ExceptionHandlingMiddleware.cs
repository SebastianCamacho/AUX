using AutoMapper;
using BLL.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Pasa al siguiente middleware
            }
            catch (BusinessException ex)
            {
                await HandleExceptionAsync(context, ex.Message, ex.StatusCode, ex.ErrorCode);
            }
            catch (AutoMapperMappingException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new { Message = "Error en el mapeo de objetos.", Details = ex.Message });
            }
            catch (NullReferenceException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Message = "Referencia nula detectada.", Details = ex.Message });
            }
            catch (AutoMapperConfigurationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Message = "Error en la configuración de AutoMapper.", Details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { Message = "No se encontró una configuración válida en AutoMapper.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                // Para errores inesperados, loggealos si querés
                await HandleExceptionAsync(context, "Ha ocurrido un error inesperado en el servidor.", HttpStatusCode.InternalServerError, "ERR_INTERNAL");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode, string errorCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                statusCode = (int)statusCode,
                errorCode,
                message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
