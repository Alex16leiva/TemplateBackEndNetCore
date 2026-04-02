using Aplicacion.DTOs.ConfiguracionesDTO;
using Aplicacion.Services.ConfiguracionesApp;
using Microsoft.AspNetCore.Mvc;

namespace WebServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionesController : ControllerBase
    {
        private readonly IConfiguracionesApplicationService _configuracionesAppService;

        public ConfiguracionesController(IConfiguracionesApplicationService configuracionesAppService)
        {
            _configuracionesAppService = configuracionesAppService;
        }

        [HttpPost("crear-configuracion")]
        public async Task<IActionResult> CrearConfiguracion(ConfiguracionesRequest request)
        {
            var configuracion = await _configuracionesAppService.CrearConfiguracion(request);
            return Ok(configuracion);
        }

        [HttpPost("obtener-configuraciones")]
        public async Task<IActionResult> GetConfiguraciones(ConfiguracionesRequest request)
        {
            var configuraciones = await _configuracionesAppService.ObtenerConfiguracionesPaginado(request);
            return Ok(configuraciones);
        }

        [HttpPost("crear-configuracion-detalle")]
        public async Task<IActionResult> CrearConfiguracionesDetalle(ConfiguracionesRequest request)
        {
            var configuracionesDetalle = await _configuracionesAppService.CrearConfiguracionDetalle(request);
            return Ok(configuracionesDetalle);
        }

        [HttpPost("editar-configuracion-detalle")]
        public async Task<IActionResult> EditarConfiguracionesDetalle(ConfiguracionesRequest request)
        {
            var configuracionesDetalle = await _configuracionesAppService.EditarConfiguracionesDetalle(request);
            return Ok(configuracionesDetalle);
        }

        [HttpPost("editar-configuracion")]
        public async Task<IActionResult> EditarConfiguracion(ConfiguracionesRequest request)
        {
            var configuracion = await _configuracionesAppService.EditarConfiguracion(request);
            return Ok(configuracion);
        }
    }
}
