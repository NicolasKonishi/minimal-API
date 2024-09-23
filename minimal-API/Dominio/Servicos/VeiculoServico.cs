using Microsoft.EntityFrameworkCore;
using minimal_API.Dominio.Entidades;
using minimal_API.Dominio.Interfaces;
using minimal_API.Infraestrutura.DB;

namespace minimal_API.Dominio.Servicos
{
    public class VeiculoServico : IVeiculosServico
    {
        private readonly DBContexto _contexto;
        public VeiculoServico(DBContexto contexto)
        {
            _contexto = contexto;
        }


        public void Apagar(Veiculo veiculo)
        {
            _contexto.veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public Veiculo? BuscarPorId(int id)
        {
            return _contexto.veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _contexto.veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            var query = _contexto.veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome}/%")); 


            }

            int itensPorPagina = 10;
            if (pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
            }
            return query.ToList();
        }
    }
}
