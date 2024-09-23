using minimal_API.Dominio.Enuns;

namespace minimal_API.Dominio.DTO
{
    public class AdmDTO
    {
        public string Email { get; set; } = default!;

        public string Senha { get; set; } = default!;

        public Perfil? Perfil { get; set; } = default!;


    }
}
