using Microsoft.EntityFrameworkCore;
using minimal_API.Dominio.Entidades;

namespace minimal_API.Infraestrutura.DB
{
    public class DBContexto : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings;

        public DBContexto(IConfiguration configuracaoAppSettings)
        {
            _configuracaoAppSettings = configuracaoAppSettings;
        }

        public DbSet<Adm> adms { get; set; } = default!;
        public DbSet<Veiculo> veiculos { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adm>().HasData(
             new Adm
             {
                 Id = 1,
                 Email= "Adm@teste.com",
                 Senha= "123",
                 Perfil= "Adm"
             }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("conexao")?.ToString();
                if (!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseSqlServer(stringConexao);

                }
            }
        }
    }
}
