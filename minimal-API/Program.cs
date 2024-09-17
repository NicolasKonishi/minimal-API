using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using minimal_API.Dominio.DTO;
using minimal_API.Dominio.Entidades;
using minimal_API.Dominio.Interfaces;
using minimal_API.Dominio.ModelViews;
using minimal_API.Dominio.Servicos;
using minimal_API.Infraestrutura.DB;

#region builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServicos>();
builder.Services.AddScoped<IVeiculosServico, VeiculoServico>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexao"));
});

var app = builder.Build();
#endregion

#region Home

app.MapGet("/", () => Results.Json(new Home()));

#endregion

#region adms

app.MapPost("/adms/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    if(administradorServico.Login(loginDTO) != null )
    {
        return Results.Ok("login com sucesso");
    }
    else
    {
        return Results.Unauthorized();
    }
});
#endregion


#region veiculos

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculoServico) =>
{

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano

    };

    veiculoServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}",veiculo);


});

#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();
app.Run();

#endregion
