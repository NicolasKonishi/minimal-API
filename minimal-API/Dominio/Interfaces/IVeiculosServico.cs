using minimal_API.Dominio.Entidades;

namespace minimal_API.Dominio.Interfaces
{
    public interface IVeiculosServico
    {
        List<Veiculo> Todos(int pagina = 1, string? nome=null, string? marca=null);
        Veiculo? BuscarPorId(int id);

        void Incluir(Veiculo veiculo);

        void Atualizar(Veiculo veiculo);

        void Apagar(Veiculo veiculo);
    }
}
