namespace Aplicacion.DTOs.Seguridad
{
    public class UserRequest
    {
        public string? UsuarioId { get; set; }
        public string? Password { get; set; }
    }

    public class CreateUserRequest : RequestBase
    {
        public UsuarioDTO? Usuario { get; set; }
    }
}
