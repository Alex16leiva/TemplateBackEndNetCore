using Aplicacion.Core;
using Aplicacion.DTOs;
using Aplicacion.DTOs.Seguridad;
using Aplicacion.Helpers;
using AutoMapper;
using Dominio.Context.Entidades;
using Dominio.Context.Entidades.Seguridad;
using Dominio.Core;
using Dominio.Core.Extensions;
using Infraestructura.Context;
using Infraestructura.Core.Jwtoken;

namespace Aplicacion.Services
{
    public class SecurityAplicationService : BaseDisposable
    {
        private readonly IGenericRepository<IDataContext> _genericRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public SecurityAplicationService(IGenericRepository<IDataContext> genericRepository, ITokenService tokenService, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public UsuarioDTO EditarUsuario(EdicionUsuarioRequest request)
        {
            string mensajeValidacion = request.Usuario.ValidarCampos();

            if (mensajeValidacion.HasValue())
            {
                return new UsuarioDTO
                {
                    Message = mensajeValidacion,
                };
            }

            Usuario usuarioExiste = _genericRepository.GetSingle<Usuario>(r => r.UsuarioId == request.Usuario.UsuarioId);

            if (usuarioExiste.IsNull())
            {
                return new UsuarioDTO
                {
                    Message = "El usuario no existe"
                };
            }

            if (request.Usuario.EditarContrasena)
            {
                usuarioExiste.Contrasena = PasswordEncryptor.Encrypt(request.Usuario.Contrasena);
            }

            usuarioExiste.Nombre = request.Usuario.Nombre.ValueOrEmpty();
            usuarioExiste.Apellido = request.Usuario.Apellido.ValueOrEmpty();
            usuarioExiste.RolId = request.Usuario.RolId.ValueOrEmpty();
            usuarioExiste.Activo = request.Usuario.Activo;

            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("EditarUsuario");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return new UsuarioDTO();
        }

        public List<PantallaDTO> ObtenerPantallas()
        {
            var pantallas = _genericRepository.GetAll<Pantalla>();
            return pantallas.Select(r => new PantallaDTO { Descripcion = r.Descripcion, PantallaId = r.PantallaId }).ToList();
        }

        public RolDTO EdicionPermisos(EdicionPermisosRequest request)
        {
            var permisos = _genericRepository.GetFiltered<Permisos>(r => r.RolId == request.RolId);

            foreach (var item in request.Permisos)
            {
                var permiso = permisos.FirstOrDefault(r => r.PantallaId == item.PantallaId);
                if (permiso.IsNotNull())
                {
                    permiso.Ver = item.Ver;
                    permiso.Editar = item.Editar;
                    permiso.Eliminar = item.Eliminar;

                    if (!permiso.Ver)
                    {
                        _genericRepository.Remove(permiso);
                    }
                }
                else
                {
                    var nuevoPermiso = new Permisos
                    {
                        Editar = item.Editar,
                        Eliminar = item.Eliminar,
                        PantallaId = item.PantallaId,
                        RolId = item.RolId,
                        Ver = item.Ver,
                    };
                    _genericRepository.Add(nuevoPermiso);
                }


                TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("AgregarUsuario");
                _genericRepository.UnitOfWork.Commit(transactionInfo);
            }
            return new RolDTO { };
        }

        public UsuarioDTO CrearUsuario(EdicionUsuarioRequest request)
        {
            string mensajeValidacion = request.Usuario.ValidarCampos();

            if (mensajeValidacion.HasValue())
            {
                return new UsuarioDTO
                {
                    Message = mensajeValidacion,
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
                Nombre = request.Usuario.Nombre.ValueOrEmpty(),
                RolId = request.Usuario.RolId.ValueOrEmpty(),
                UsuarioId = request.Usuario.UsuarioId.ValueOrEmpty(),
                Activo = request.Usuario.Activo
            };

            _genericRepository.Add(usuario);
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("AgregarUsuario");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public UsuarioDTO IniciarSesion(UserRequest request)
        {
            List<string> includes = ["Rol", "Rol.Permisos"];

            string passwordEncrypted = PasswordEncryptor.Encrypt(request?.Password);

            Usuario usuario = _genericRepository.GetSingle<Usuario>(r => r.UsuarioId == request.UsuarioId && r.Contrasena == passwordEncrypted, includes);

            if (usuario.IsNotNull())
            {
                if (!usuario.Activo)
                {
                    return new UsuarioDTO { Message = $"Usuario {usuario.UsuarioId} esta desactivado" };
                }
                return new UsuarioDTO
                {
                    Apellido = usuario.Apellido,
                    Nombre = usuario.Nombre,
                    RolId = usuario.RolId,
                    Token = _tokenService.Generate(usuario),
                    UsuarioAutenticado = true,
                    UsuarioId = usuario.UsuarioId,
                    Permisos = MapPermisosDto(usuario.Rol?.Permisos)
                };
            }

            return new UsuarioDTO
            {
                Message = "Usuario o Contraseña no valido",
                UsuarioAutenticado = false
            };
        }

        public SearchResult<UsuarioDTO> ObtenerUsuario(GetUserRequest request)
        {
            var dynamicFilter = DynamicFilterFactory.CreateDynamicFilter(request.QueryInfo);
            var usuarios = _genericRepository.GetPagedAndFiltered<Usuario>(dynamicFilter);

            return new SearchResult<UsuarioDTO>
            {
                PageCount = usuarios.PageCount,
                ItemCount = usuarios.ItemCount,
                TotalItems = usuarios.TotalItems,
                PageIndex = usuarios.PageIndex,
                Items = (from qry in usuarios.Items as IEnumerable<Usuario> select MapUsuarioDto(qry)).ToList(),
            };

        }

        public RolDTO CrearRol(EdicionRolRequest request)
        {
            var rol = _genericRepository.GetSingle<Rol>(r => r.RolId == request.Rol.RolId);
            if (rol.IsNotNull())
            {
                return new RolDTO
                {
                    Message = $"El rol {request.Rol.RolId} ya existe"
                };
            }

            var nuevoRol = new Rol
            {
                Descripcion = request.Rol.Descripcion,
                RolId = request.Rol.RolId
            };

            _genericRepository.Add(nuevoRol);
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("AgregarRol");
            _genericRepository.UnitOfWork.Commit(transactionInfo);

            return new RolDTO();
        }

        public RolDTO EditarRol(EdicionRolRequest request)
        {
            var rol = _genericRepository.GetSingle<Rol>(r => r.RolId == request.Rol.RolId);

            if (rol.IsNull())
            {
                return new RolDTO
                {
                    Message = $"El Rol {request.Rol.RolId} no existe"
                };
            }

            rol.Descripcion = request.Rol.Descripcion;
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("EditarRol");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return new RolDTO();
        }

        public List<RolDTO> ObtenerRoles()
        {
            var includes = new List<string> { "Permisos" };
            var roles = _genericRepository.GetAll<Rol>(includes);

            return roles.Select(qry =>
            new RolDTO
            {
                Descripcion = qry.Descripcion,
                RolId = qry.RolId,
                Permisos = MapPermisosDto(qry?.Permisos),
            }).ToList();
        }

        private static List<PermisosDTO> MapPermisosDto(List<Permisos>? permisos)
        {
            return permisos.Select(r => new PermisosDTO
            {
                Editar = r.Editar,
                Eliminar = r.Eliminar,
                PantallaId = r.PantallaId,
                RolId = r.RolId,
                Ver = r.Ver,
            }).ToList();
        }

        private static UsuarioDTO MapUsuarioDto(Usuario qry)
        {
            return new UsuarioDTO
            {
                Apellido = qry.Apellido,
                Contrasena = qry.Contrasena,
                Nombre = qry.Nombre,
                RolId = qry.RolId,
                UsuarioId = qry.UsuarioId,
                FechaTransaccion = qry.FechaTransaccion,
                Activo = qry.Activo
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_genericRepository.IsNotNull()) _genericRepository.Dispose();

            }

            base.Dispose(disposing);
        }
    }
}
