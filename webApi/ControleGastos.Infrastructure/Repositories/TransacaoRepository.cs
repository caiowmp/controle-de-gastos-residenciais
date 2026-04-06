using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Domain.Entities;
using ControleGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories
{
  internal class TransacaoRepository(ControleGastosDbContext _dbContext) : ITransacaoReadOnly, ITransacaoWriteOnly
  {
    public async Task Add(Transacao transacao)
    {
      await _dbContext.Transacoes.AddAsync(transacao);
    }

    public async Task<List<Transacao>> GetAll()
    {
      return await _dbContext.Transacoes
        .AsNoTracking()
        .ToListAsync();
    }
  }
}
