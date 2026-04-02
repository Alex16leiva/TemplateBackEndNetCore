using Aplicacion.DTOs;
using Aplicacion.DTOs.ConfiguracionesDTO;

namespace Aplicacion.Services.ConfiguracionesApp
{
    public interface IConfiguracionesApplicationService
    {
        Task<SearchResult<ConfiguracionesDTO>> ObtenerConfiguracionesPaginado(ConfiguracionesRequest request);
        Task<ConfiguracionesDTO> CrearConfiguracion(ConfiguracionesRequest request);
        Task<ConfiguracionesDTO> EditarConfiguracion(ConfiguracionesRequest request);
        Task<ConfiguracionesDetalleDTO> CrearConfiguracionDetalle(ConfiguracionesRequest request);
        Task<ConfiguracionesDetalleDTO> EditarConfiguracionesDetalle(ConfiguracionesRequest request);

    }
}