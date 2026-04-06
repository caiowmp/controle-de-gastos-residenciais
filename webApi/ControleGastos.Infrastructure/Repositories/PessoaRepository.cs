using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Domain.Entities;
using ControleGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories
{
  internal class PessoaRepository(ControleGastosDbContext _dbContext) : IPessoaReadOnly, IPessoaWriteOnly, IPessoaUpdateOnly
  {
    public async Task Add(Pessoa pessoa)
    {
      await _dbContext.Pessoas.AddAsync(pessoa);
    }

    public async Task Delete(Guid id)
    {
      var result = await _dbContext.Pessoas.FindAsync(id);

      _dbContext.Pessoas.Remove(result!);
    }

    public async Task<List<Pessoa>> GetAll()
    {
      return await _dbContext.Pessoas
        .AsNoTracking()
        .ToListAsync();
    }

    public void Update(Pessoa pessoa)
    {
      _dbContext.Pessoas.Update(pessoa);
    }
  }
}
