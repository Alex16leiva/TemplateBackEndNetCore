namespace Aplicacion.DTOs.Seguridad
{
    public class RolDTO : ResponseBase
    {
        public string? RolId { get; set; }
        public string? Descripcion { get; set; }
        public List<PermisosDTO>? Permisos { get; set; }
    }
}
