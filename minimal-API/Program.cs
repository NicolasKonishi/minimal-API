using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using minimal_API.Dominio.DTO;
using minimal_API.Dominio.Entidades;
using minimal_API.Dominio.Enuns;
using minimal_API.Dominio.Interfaces;
using minimal_API.Dominio.ModelViews;
using minimal_API.Dominio.Servicos;
using minimal_API.Infraestrutura.DB;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using minimal_API.Migrations;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

#region builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();

if (string.IsNullOrEmpty(key))
    key = "123456";


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();



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

app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");

#endregion

#region adms

string GerarTokenJwt(Adm adm)
{
    if(string.IsNullOrEmpty(key)) return string.Empty;
    
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        expires = DateTime.Now.Day(1)
        );
}


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
}).WithTags("Administrador");

app.MapGet("/adms", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
{
    var adms = new List<AdministradorModelView>();
    var administradores = administradorServico.Todos(pagina);
    foreach(var adm in administradores)
    {
        adms.Add(new AdministradorModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }

}).RequireAuthorization().WithTags("Administrador");

app.MapGet("/adms/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{

    var administrador= administradorServico.BuscaPorId(id);

    if (administrador == null) return Results.NotFound();

    return Results.Ok(new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    });


}).RequireAuthorization().WithTags("Administrador");

app.MapPost("/adms", ([FromBody] AdmDTO admDTO, IAdministradorServico administradorServico) =>
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };


    if (string.IsNullOrEmpty(admDTO.Email))
        validacao.Mensagens.Add("Email não pode ser vazio");
    if (string.IsNullOrEmpty(admDTO.Senha))
        validacao.Mensagens.Add("Senha não pode ser vazia");
    if (admDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil não pode ser vazio");

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

       var adm = new Adm
        {
            Email = admDTO.Email,
            Senha = admDTO.Senha,
            Perfil = admDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
        };


    administradorServico.Incluir(adm);
    
    return Results.Created($"/administrador/{adm.Id}", new AdministradorModelView
    {
        Id = adm.Id,
        Email = adm.Email,
        Perfil = adm.Perfil
    });

}).RequireAuthorization().WithTags("Administrador");
#endregion

#region veiculos
ErrosDeValidacao ValidaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };


    if (string.IsNullOrEmpty(veiculoDTO.Nome))
    {
        validacao.Mensagens.Add("O nome nao pode ser vazio!");
    }
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
    {
        validacao.Mensagens.Add("A marca nao pode ficar em branco!");
    }
    if (veiculoDTO.Ano < 1950)
    {
        validacao.Mensagens.Add("O ano nao pode ser menor que 1950!");
    }

    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculoServico) =>
{
    var validacao = ValidaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0)
    {
        return Results.BadRequest(validacao);
    }

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano

    };

    veiculoServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}",veiculo);


}).RequireAuthorization().WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) =>
{

    var veiculos = veiculosServico.Todos(pagina);


    return Results.Ok(veiculos);


}).RequireAuthorization().WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{

    var veiculo = veiculosServico.BuscarPorId(id);

    if(veiculo == null) return Results.NotFound();

    return Results.Ok(veiculo);


}).RequireAuthorization().WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id,VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscarPorId(id);
    if (veiculo == null) return Results.NotFound();

    var validacao = ValidaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);
    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca= veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculosServico.Atualizar(veiculo);
    return Results.Ok(veiculo);


}).RequireAuthorization().WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id,IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscarPorId(id);
    if (veiculo == null) return Results.NotFound();
    veiculosServico.Apagar(veiculo);

    return Results.NoContent();


}).RequireAuthorization().WithTags("Veiculos");

#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication(); //a ordem importa pela logica
app.UseAuthorization(); 

app.Run();

#endregion
