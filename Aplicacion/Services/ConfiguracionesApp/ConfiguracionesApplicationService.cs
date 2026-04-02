using Aplicacion.DTOs;
using Aplicacion.DTOs.ConfiguracionesDTO;
using Aplicacion.Helpers;
using Dominio.Context.Entidades.ConfiguracionesAgg;
using Dominio.Core;
using Dominio.Core.Extensions;
using Infraestructura.Context;

namespace Aplicacion.Services.ConfiguracionesApp
{
    public class ConfiguracionesApplicationService : IConfiguracionesApplicationService
    {
        private readonly IGenericRepository<IDataContext> _genericRepository;

        public ConfiguracionesApplicationService(IGenericRepository<IDataContext> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<ConfiguracionesDTO> CrearConfiguracion(ConfiguracionesRequest request)
        {
            var existingConfiguracion = await _genericRepository.GetSingleAsync<Configuraciones>(x => x.ConfiguracionId == request.Configuraciones.ConfiguracionId);
            if (existingConfiguracion.IsNotNull()) {
                return new ConfiguracionesDTO
                {
                    ConfiguracionId = existingConfiguracion.ConfiguracionId,
                    Descripcion = existingConfiguracion.Descripcion,
                    Message = $"Ya existe una configuración con el ID {existingConfiguracion.ConfiguracionId}" 
                };
            }

            var configuracion = new Configuraciones
            {
                ConfiguracionId = request.Configuraciones.ConfiguracionId,
                Descripcion = request.Configuraciones.Descripcion
            };
            await _genericRepository.AddAsync(configuracion);
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("CrearConfiguracion");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return mapConfiguracionesDTO(configuracion);
        }

        public async Task<ConfiguracionesDetalleDTO> EditarConfiguracionesDetalle(ConfiguracionesRequest request)
        {
            var existingConfiguracionDetalle = await _genericRepository.GetSingleAsync<ConfiguracionesDetalle>(x => x.ConfiguracionId == request.ConfiguracionesDetalle.ConfiguracionId && x.Atributo == request.ConfiguracionesDetalle.Atributo);
            if (existingConfiguracionDetalle.IsNull())
            {
                return new ConfiguracionesDetalleDTO
                {
                    ConfiguracionId = request.ConfiguracionesDetalle.ConfiguracionId,
                    Atributo = request.ConfiguracionesDetalle.Atributo,
                    Descripcion = request.ConfiguracionesDetalle.Descripcion,
                    Valor = request.ConfiguracionesDetalle.Valor,
                    Message = $"No existe un detalle de configuración con el ID {request.ConfiguracionesDetalle.ConfiguracionId} y el atributo {request.ConfiguracionesDetalle.Atributo}"
                };
            }
            existingConfiguracionDetalle.Descripcion = request.ConfiguracionesDetalle.Descripcion;
            existingConfiguracionDetalle.Valor = request.ConfiguracionesDetalle.Valor;

            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("EditarConfiguracionDetalle");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return mapConfiguracionesDetalleDTO(existingConfiguracionDetalle);
        }

        public async Task<ConfiguracionesDTO> EditarConfiguracion(ConfiguracionesRequest request)
        {
            var existingConfiguracion = await _genericRepository.GetSingleAsync<Configuraciones>(x => x.ConfiguracionId == request.Configuraciones.ConfiguracionId);
            if (existingConfiguracion.IsNull())
            {
                return new ConfiguracionesDTO
                {
                    ConfiguracionId = request.Configuraciones.ConfiguracionId,
                    Descripcion = request.Configuraciones.Descripcion,
                    Message = $"No existe una configuración con el ID {request.Configuraciones.ConfiguracionId}"
                };
            }
            existingConfiguracion.Descripcion = request.Configuraciones.Descripcion;

            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("EditarConfiguracion");
            _genericRepository.UnitOfWork.Commit(transactionInfo);

            return mapConfiguracionesDTO(existingConfiguracion);
        }

        public async Task<ConfiguracionesDetalleDTO> CrearConfiguracionDetalle(ConfiguracionesRequest request)
        {
            var existingConfiguracion = await _genericRepository.GetSingleAsync<Configuraciones>(x => x.ConfiguracionId == request.ConfiguracionesDetalle.ConfiguracionId);
            if (existingConfiguracion.IsNull())
            {
                return new ConfiguracionesDetalleDTO
                {
                    ConfiguracionId = request.ConfiguracionesDetalle.ConfiguracionId,
                    Descripcion = request.ConfiguracionesDetalle.Descripcion,
                    Message = $"La configuración con el ID {request.ConfiguracionesDetalle.ConfiguracionId} no existe"
                };
            }

            var configuracionesDetalle = new ConfiguracionesDetalle
            {
                ConfiguracionId = request.ConfiguracionesDetalle.ConfiguracionId,
                Atributo = request.ConfiguracionesDetalle.Atributo,
                Descripcion = request.ConfiguracionesDetalle.Descripcion,
                Valor = request.ConfiguracionesDetalle.Valor,
            };

            await _genericRepository.AddAsync(configuracionesDetalle);
            TransactionInfo transactionInfo = request.RequestUserInfo.CrearTransactionInfo("CrearConfiguracionDetalle");
            _genericRepository.UnitOfWork.Commit(transactionInfo);
            return mapConfiguracionesDetalleDTO(configuracionesDetalle);
        }

        private static ConfiguracionesDetalleDTO mapConfiguracionesDetalleDTO(ConfiguracionesDetalle configuracionesDetalle)
        {
            if (configuracionesDetalle.IsNull()) return null;
            return new ConfiguracionesDetalleDTO
            {
                ConfiguracionId = configuracionesDetalle.ConfiguracionId,
                Atributo = configuracionesDetalle.Atributo,
                Descripcion = configuracionesDetalle.Descripcion,
                Valor = configuracionesDetalle.Valor,
            };
        }

        public async Task<SearchResult<ConfiguracionesDTO>> ObtenerConfiguracionesPaginado(ConfiguracionesRequest request)
        {
            var dynamicFilter = DynamicFilterFactory.CreateDynamicFilter(request.QueryInfo);
            var configuraciones = await _genericRepository.GetPagedAndFilteredAsync<Configuraciones>(dynamicFilter);
            return new SearchResult<ConfiguracionesDTO>
            {
                ItemCount = configuraciones.ItemCount,
                PageCount = configuraciones.PageCount,
                PageIndex = configuraciones.PageIndex,
                TotalItems = configuraciones.TotalItems,
                Items = (from query in configuraciones.Items as IEnumerable<Configuraciones> select mapConfiguracionesDTO(query) ).ToList()
            };
        }

        private static ConfiguracionesDTO mapConfiguracionesDTO(Configuraciones query)
        {
            if (query.IsNull()) return null;
            return new ConfiguracionesDTO
            {
                ConfiguracionId = query.ConfiguracionId,
                Descripcion = query.Descripcion,
                ConfiguracionesDetalle = query.ConfiguracionesDetalle?
                    .Select(detalle => mapConfiguracionesDetalleDTO(detalle))
                    .ToList() ?? new List<ConfiguracionesDetalleDTO>()
            };
        }
    }
}
