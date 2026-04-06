using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Domain.Entities;
using ControleGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories
{
  /// <summary>
  /// Repositório para operações de leitura e escrita da entidade Transacao.
  /// Gerencia transações financeiras (receitas e despesas) do sistema.
  /// </summary>
  internal class TransacaoRepository(ControleGastosDbContext _dbContext) : ITransacaoReadOnly, ITransacaoWriteOnly
  {
    /// <summary>
    /// Adiciona uma nova transação ao banco de dados.
    /// A transação sempre está associada a uma pessoa e uma categoria.
    /// </summary>
    /// <param name="transacao">A transação a ser adicionada</param>
    public async Task Add(Transacao transacao)
    {
      await _dbContext.Transacoes.AddAsync(transacao);
      await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// Inclui os dados de navegação da Pessoa e Categoria associadas.
    /// Utiliza AsNoTracking para melhorar performance em operações de leitura.
    /// </summary>
    /// <returns>Lista de todas as transações</returns>
    public async Task<List<Transacao>> GetAll()
    {
      return await _dbContext.Transacoes
        .Include(t => t.Pessoa)
        .Include(t => t.Categoria)
        .AsNoTracking()
        .ToListAsync();
    }

    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="pessoaId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    public async Task<List<Transacao>> GetByPessoaId(Guid pessoaId)
    {
      return await _dbContext.Transacoes
        .Where(t => t.PessoaId == pessoaId)
        .Include(t => t.Categoria)
        .AsNoTracking()
        .ToListAsync();
    }

    /// <summary>
    /// Obtém uma transação específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da transação a ser obtida</param>
    /// <returns>A transação encontrada ou null se não existir</returns>
    public async Task<Transacao?> GetById(Guid id)
    {
      return await _dbContext.Transacoes
        .Include(t => t.Pessoa)
        .Include(t => t.Categoria)
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id);
    }
  }
}
