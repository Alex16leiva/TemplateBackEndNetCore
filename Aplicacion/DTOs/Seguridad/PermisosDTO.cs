namespace Aplicacion.DTOs.Seguridad
{
    public class PermisosDTO : ResponseBase
    {
        public string? RolId { get; set; }
        public string? PantallaId { get; set; }
        public bool Ver { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
    }
}
