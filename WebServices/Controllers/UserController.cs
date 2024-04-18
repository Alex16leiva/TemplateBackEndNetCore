using Aplicacion.DTOs.Seguridad;
using Aplicacion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Route("")]
        [HttpGet]
        public UsuarioDTO Login([FromQuery] UserRequest request)
        {
            UsuarioDTO usuario = _securityAppService.IniciarSesion(request);

            return usuario;
        }

        [Authorize]
        [Route("")]
        [HttpPost]
        public UsuarioDTO CreateUser(CreateUserRequest request)
        {
            UsuarioDTO usuario = _securityAppService.CrearUsuario(request);

            return usuario ;
        }
    }
}
