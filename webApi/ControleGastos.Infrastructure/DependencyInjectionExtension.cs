using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Application.Interfaces.Services;
using ControleGastos.Application.Services;
using ControleGastos.Infrastructure.Data;
using ControleGastos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControleGastos.Infrastructure
{
  /// <summary>
  /// Extensão para configuração de Dependency Injection da camada Infrastructure.
  /// Registra o DbContext, repositórios e serviços da aplicação.
  /// </summary>
  public static class DependencyInjection
  {
    /// <summary>
    /// Registra todos os serviços e dependências da Infrastructure.
    /// Deve ser chamado no Program.cs da API.
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      AddDbContext(services, configuration);
      AddRepositories(services);
      AddServices(services);
    }

    /// <summary>
    /// Registra os repositórios usando o padrão CQRS (Read/Write).
    /// Cada repositório implementa as interfaces segregadas de leitura e escrita.
    /// </summary>
    private static void AddRepositories(IServiceCollection services)
    {
      // Pessoa Repository
      services.AddScoped<IPessoaReadOnly, PessoaRepository>();
      services.AddScoped<IPessoaWriteOnly, PessoaRepository>();
      services.AddScoped<IPessoaUpdateOnly, PessoaRepository>();

      // Categoria Repository
      services.AddScoped<ICategoriaReadOnly, CategoriaRepository>();
      services.AddScoped<ICategoriaWriteOnly, CategoriaRepository>();

      // Transacao Repository
      services.AddScoped<ITransacaoReadOnly, TransacaoRepository>();
      services.AddScoped<ITransacaoWriteOnly, TransacaoRepository>();
    }

    /// <summary>
    /// Registra os serviços de aplicação que implementam a lógica de negócio.
    /// Os serviços utilizam os repositórios para acessar dados.
    /// </summary>
    private static void AddServices(IServiceCollection services)
    {
      services.AddScoped<IPessoaService, PessoaService>();
      services.AddScoped<ICategoriaService, CategoriaService>();
      services.AddScoped<ITransacaoService, TransacaoService>();
      services.AddScoped<IRelatorioService, RelatorioService>();
    }

    /// <summary>
    /// Configura o Entity Framework Core com SQLite.
    /// A connection string é obtida do appsettings.json.
    /// </summary>
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ControleGastosDbContext>(options =>
          options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
  }
}

