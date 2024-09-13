using Microsoft.EntityFrameworkCore;
using minimal_API.Dominio.DTO;
using minimal_API.Infraestrutura.DB;


var builder = WebApplication.CreateBuilder(args);


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

app.MapPost("/login", (LoginDTO loginDto) =>
{
    if(loginDto.Email == "adm@gmail.com" && loginDto.Senha == "1234")
    {
        return Results.Ok("login com sucesso");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.Run();
