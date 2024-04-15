namespace Aplicacion.DTOs.Seguridad
{
    public class UsuarioDTO : ResponseBase
    {
        public string UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contrasena { get; set; }
        public string Token { get; set; }
    }
}
