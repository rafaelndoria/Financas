using Financas.Data;
using Financas.Repositories;
using Financas.Repositories.Interfaces;
using Financas.Services;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SegurancaJWT.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnectionProvider>(_ =>
    new DbConnectionProvider(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Financas;Integrated Security=True;Connect Timeout=30;Encrypt=False;"));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<ICartaoRepository, CartaoRepository>();
builder.Services.AddScoped<ITipoOpCartaoRepository, TipoOpCartaoRepository>();
builder.Services.AddScoped<ITipoOpRepository, TipoOpRepository>();
builder.Services.AddScoped<IOpCartaoRepository, OpCartaoRepository>();
builder.Services.AddScoped<IFaturaRepository, FaturaRepository>();
builder.Services.AddScoped<ITituloRepository, TituloRepository>();
builder.Services.AddScoped<IPagamentoFaturaRepository, PagamentoFaturaRepository>();

builder.Services.AddTransient<JwtTokenService>();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:secrectKey"]))
    };
});

builder.Services.AddScoped<CryptService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
