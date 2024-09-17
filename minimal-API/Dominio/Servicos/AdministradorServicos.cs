using Microsoft.EntityFrameworkCore;
using minimal_API.Dominio.DTO;
using minimal_API.Dominio.Entidades;
using minimal_API.Dominio.Interfaces;
using minimal_API.Infraestrutura.DB;

namespace minimal_API.Dominio.Servicos
{
    public class AdministradorServicos : IAdministradorServico
    {
        private readonly DBContexto _contexto;
        public AdministradorServicos(DBContexto contexto)
        {
            _contexto = contexto;
        }

        public Adm? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.adms.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }

    }
}
