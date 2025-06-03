using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores de negocio controlados.
    /// Lanza esta excepción cuando una operación viola reglas lógicas o de validación del dominio.
    /// </summary>
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string ErrorCode { get; }

        public BusinessException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest; // 400 por defecto
        }

        public BusinessException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public BusinessException(string message, string errorCode)
            : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ErrorCode = errorCode;
        }

        public BusinessException(string message, string errorCode, HttpStatusCode statusCode)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        // Constructor 6: message, statusCode, innerException (¡NUEVO!)
        public BusinessException(string message, HttpStatusCode statusCode, Exception innerException)
            : base(message, innerException) // Pasa message e innerException al constructor base
        {
            StatusCode = statusCode; // Guarda el statusCode
        }

        // Constructor 7: message, errorCode, statusCode, innerException (¡NUEVO!)
        public BusinessException(string message, string errorCode, HttpStatusCode statusCode, Exception innerException)
            : base(message, innerException) // Pasa message e innerException al constructor base
        {
            ErrorCode = errorCode;    // Guarda el errorCode
            StatusCode = statusCode; // Guarda el statusCode
        }
    }
}
