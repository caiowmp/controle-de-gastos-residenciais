using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Infrastructure.Data;
using ControleGastos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControleGastos.Infrastructure
{
  public static class DependencyInjection
  {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      AddDbContext(services, configuration);
      AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
      services.AddScoped<IPessoaReadOnly, PessoaRepository>();
      services.AddScoped<IPessoaWriteOnly, PessoaRepository>();
      services.AddScoped<IPessoaUpdateOnly, PessoaRepository>();

      services.AddScoped<ICategoriaReadOnly, CategoriaRepository>();
      services.AddScoped<ICategoriaWriteOnly, CategoriaRepository>();

      services.AddScoped<ITransacaoReadOnly, TransacaoRepository>();
      services.AddScoped<ITransacaoWriteOnly, TransacaoRepository>();

    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ControleGastosDbContext>(options =>
          options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
  }
}
