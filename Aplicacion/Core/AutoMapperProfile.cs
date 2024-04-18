using Aplicacion.DTOs.Seguridad;
using AutoMapper;
using Dominio.Context.Entidades.Seguridad;

namespace Aplicacion.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
        }
    }
}
