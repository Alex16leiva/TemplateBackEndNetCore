using Dominio.Core.Extensions;
using System.Text;

namespace Aplicacion.DTOs.Seguridad
{
    public class UsuarioDTO : ResponseBase
    {
        public string? UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Contrasena { get; set; }
        public string? Token { get; set; }
        public bool UsuarioAutenticado { get; set; }
        public string? RolId { get; set; }


        public string ValidarCampos()
        {
            StringBuilder mensajeValidacion = new StringBuilder();

            if (UsuarioId.IsMissingValue())
            {
                mensajeValidacion.AppendLine("El usuarioId es requerido");
            }
            if (Nombre.IsMissingValue())
            {
                mensajeValidacion.AppendLine("El nombre es requerido");
            }
            if (Apellido.IsMissingValue()) 
            {
                mensajeValidacion.AppendLine("El apellido es requerido");
            }
            if (Contrasena.IsMissingValue())
            {
                mensajeValidacion.AppendLine("La contraña es requerida");
            }
            if (RolId.IsMissingValue())
            {
                mensajeValidacion.AppendLine("El rol es requerido");
            }

            return mensajeValidacion.ToString();
        }
    }
}
