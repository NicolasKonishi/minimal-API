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

        public Adm BuscaPorId(int id)
        {
            return _contexto.adms.Where(a => a.Id == id).FirstOrDefault();
        }


        public Adm? Incluir(Adm adm)
        {
            _contexto.adms.Add(adm);
            _contexto.SaveChanges();

            return adm;
        }

        public Adm? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.adms.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }

        public List<Adm> Todos(int? pagina)
        {
            var query = _contexto.adms.AsQueryable();

            int itensPorPagina = 10;

            if (pagina != null)
                query = query.Skip(((int)pagina -1) * itensPorPagina).Take(itensPorPagina);
            return query.ToList();
        }
    }
}
