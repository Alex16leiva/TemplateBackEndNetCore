using Dominio.Core.Extensions;

namespace Aplicacion.DTOs
{
    public abstract class ResponseBase
    {
        public string? Message { get; set; }
        public string? ValidationErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public DateTime? FechaTransaccion { get; set; }

        public bool HasValidationMessage()
        {
            return Message.HasValue();
        }

        public bool HasValidationErrorMessage()
        {
            return !string.IsNullOrWhiteSpace(ValidationErrorMessage);
        }

        public void AppendValidationErrorMessage(string message)
        {
            if (HasValidationErrorMessage())
            {
                ValidationErrorMessage = $"{ValidationErrorMessage}, {message}";
                return;
            }
            ValidationErrorMessage = message;
        }
    }
}
