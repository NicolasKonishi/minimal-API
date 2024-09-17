using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_API.Dominio.DTO;
using minimal_API.Dominio.Interfaces;
using minimal_API.Dominio.Servicos;
using minimal_API.Infraestrutura.DB;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServicos>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexao"));
});


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello world!");


app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
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


app.Run();
