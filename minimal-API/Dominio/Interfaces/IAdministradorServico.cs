using minimal_API.Dominio.DTO;
using minimal_API.Dominio.Entidades;

namespace minimal_API.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Adm? Login(LoginDTO loginDTO);
    }
}
