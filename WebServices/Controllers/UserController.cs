using Aplicacion.DTOs;
using Aplicacion.DTOs.Seguridad;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SecurityAplicationService _securityAppService;
        public UserController(SecurityAplicationService securityAppService)
        {
            _securityAppService = securityAppService;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public UsuarioDTO Login([FromForm] UserRequest request)
        {
            UsuarioDTO usuario = _securityAppService.IniciarSesion(request);

            return usuario;
        }

        [Authorize]
        [HttpPost("crear-usuario")]
        public UsuarioDTO CreateUser(EdicionUsuarioRequest request)
        {
            UsuarioDTO usuario = _securityAppService.CrearUsuario(request);

            return usuario;
        }

        [Authorize]
        [HttpPost("editar-usuario")]
        public UsuarioDTO EditarUsuario(EdicionUsuarioRequest request)
        {
            UsuarioDTO usuario = _securityAppService.EditarUsuario(request);
            return usuario;
        }

        [Authorize]
        [HttpPost("obtener-usuarios")]
        public SearchResult<UsuarioDTO> ObtenerUsuarios(GetUserRequest request)
        {
            var usuarios = _securityAppService.ObtenerUsuario(request);
            return usuarios;
        }

        [Authorize]
        [HttpGet("obtener-roles")]
        public List<RolDTO> ObtenerRoles()
        {
            var roles = _securityAppService.ObtenerRoles();
            return roles;
        }

        [Authorize]
        [HttpPost("crear-rol")]
        public RolDTO CrearRol(EdicionRolRequest request)
        {
            var rol = _securityAppService.CrearRol(request);
            return rol;
        }

        [Authorize]
        [HttpPost("editar-rol")]
        public RolDTO EditarRol(EdicionRolRequest request)
        {
            var rol = _securityAppService.EditarRol(request);
            return rol;
        }

        [Authorize]
        [HttpGet("obtener-pantalla")]
        public List<PantallaDTO> ObtenerPantalla()
        {
            var pantallas = _securityAppService.ObtenerPantallas();
            return pantallas;
        }

        [Authorize]
        [HttpPost("edicion-permisos")]
        public RolDTO EdicionPermisos(EdicionPermisosRequest request)
        {
            var rol = _securityAppService.EdicionPermisos(request);
            return rol;
        }
    }
}
