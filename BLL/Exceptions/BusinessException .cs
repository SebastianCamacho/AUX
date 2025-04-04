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
    }
}
