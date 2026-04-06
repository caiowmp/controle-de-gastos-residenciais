using ControleGastos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// CORS Configuration
// Configuração do CORS para permitir requisições do frontend React
// Em produção, altere as origens permitidas conforme necessário
builder.Services.AddCors(options =>
{
  // Política alternativa para ambiente de desenvolvimento (mais permissiva)
  if (builder.Environment.IsDevelopment())
  {
    options.AddPolicy("AllowAllDevelopment", policy =>
    {
      policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
  }
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new()
  {
    Title = "Controle de Gastos API",
    Version = "v1",
    Description = "API para gerenciamento de despesas residenciais"
  });
});

// Dependecy Injection
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// CORS - Aplicar a política de CORS antes de outras middlewares
// Em desenvolvimento, usar política permissiva
if (app.Environment.IsDevelopment())
{
  app.UseCors("AllowAllDevelopment");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
