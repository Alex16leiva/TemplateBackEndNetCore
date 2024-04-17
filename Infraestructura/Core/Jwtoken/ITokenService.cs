using Dominio.Context.Entidades.Seguridad;

namespace Infraestructura.Core.Jwtoken
{
    public interface ITokenService
    {
        string Generate(Usuario user);
    }
}
