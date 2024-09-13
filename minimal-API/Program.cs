using minimal_API.Dominio.DTO;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
