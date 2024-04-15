using Aplicacion.DTOs.Seguridad;
using Aplicacion.Helpers;
using Dominio.Context.Entidades.Seguridad;
using Dominio.Core;
using Infraestructura.Context;

namespace Aplicacion.Services
{
    public class SecurityAplicationService
    {
        private readonly IGenericRepository<IDataContext> _genericRepository;

        public SecurityAplicationService(IGenericRepository<IDataContext> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public UsuarioDTO CrearUsuario(CreateUserRequest request)
        {
            var usuario = new Usuario
            {
                Apellido = "castellanos",
                Contrasena = "1",
                Nombre = "Ashlee",
                RolId = "1",
                UsuarioId = "ashlee",
            };
            var rol = new Rol
            {
                RolId = "Admin",
                Descripcion = "Administrador",
            };

            _genericRepository.Add(rol);
            _genericRepository.Add(usuario);
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("AgregarUsuario");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return new UsuarioDTO();
        }

        public UsuarioDTO IniciarSesion(UserRequest request)
        {
            List<string> includes = new List<string> { "Rol" };
            Usuario usuario = _genericRepository.GetSingle<Usuario>(r => r.UsuarioId == request.UsuarioId && r.Contrasena == request.Password);

            if (usuario != null)
            {
                return new UsuarioDTO
                {
                    UsuarioId = usuario.UsuarioId,
                    Apellido = usuario.Apellido,
                    Nombre = usuario.Nombre,
                };
            }

            return new UsuarioDTO
            {
                Message = "Usuario o Contraseña no valido"
            };
        }
    }
}
