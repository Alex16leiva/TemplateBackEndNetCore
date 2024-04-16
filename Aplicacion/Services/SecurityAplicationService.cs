using Aplicacion.DTOs.Seguridad;
using Aplicacion.Helpers;
using Dominio.Context.Entidades;
using Dominio.Context.Entidades.Seguridad;
using Dominio.Core;
using Dominio.Core.Extensions;
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
            string mensajeValidacion = request.Usuario.ValidarCampos();

            if (mensajeValidacion.HasValue())
            {
                return new UsuarioDTO
                {
                    Message = request.Usuario.Message,
                };
            }

            Usuario usuarioExiste = _genericRepository.GetSingle<Usuario>(r => r.UsuarioId == request.Usuario.UsuarioId);

            if (usuarioExiste.IsNotNull())
            {
                return new UsuarioDTO
                {
                    Message = "Usuario ya esta registrado"
                };
            }

            var usuario = new Usuario
            {
                Apellido = request.Usuario.Apellido.ValueOrEmpty(),
                Contrasena = PasswordEncryptor.Encrypt(request.Usuario.Contrasena),
                Nombre = request.Usuario.Apellido.ValueOrEmpty(),
                RolId = request.Usuario.RolId.ValueOrEmpty(),
                UsuarioId = request.Usuario.UsuarioId.ValueOrEmpty(),
            };
            
            _genericRepository.Add(usuario);
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("AgregarUsuario");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return new UsuarioDTO();
        }

        public UsuarioDTO IniciarSesion(UserRequest request)
        {
            List<string> includes = new List<string> { "Rol" };

            string passwordEncrypted = PasswordEncryptor.Encrypt(request.Password);

            Usuario usuario = _genericRepository.GetSingle<Usuario>(r => r.UsuarioId == request.UsuarioId && r.Contrasena == passwordEncrypted);

            if (usuario.IsNotNull())
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
