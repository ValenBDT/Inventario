using Inventory.APIAuthorization.Services;
using Inventory.APIAuthorization.Services.Interfaces;
using Inventory.Persistence;
using Inventory.Persistence.Interfaces;
using Inventory.Persistence.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthContextSqlServer(builder.Configuration, "DefaultConnection");
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();



app.Run();
