using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Domain.Entities;
using ControleGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories
{
  /// <summary>
  /// Repositório para operações de leitura e escrita da entidade Pessoa.
  /// Implementa o padrão CQRS com interfaces separadas de leitura e escrita.
  /// </summary>
  internal class PessoaRepository(ControleGastosDbContext _dbContext) : IPessoaReadOnly, IPessoaWriteOnly, IPessoaUpdateOnly
  {
    /// <summary>
    /// Adiciona uma nova pessoa ao banco de dados.
    /// Quando uma pessoa é adicionada, automaticamente será criada a relação em cascata com transações.
    /// </summary>
    /// <param name="pessoa">A pessoa a ser adicionada</param>
    public async Task Add(Pessoa pessoa)
    {
      await _dbContext.Pessoas.AddAsync(pessoa);
      await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deleta uma pessoa pelo seu identificador.
    /// Quando uma pessoa é deletada, todas suas transações associadas serão removidas automaticamente (cascata).
    /// </summary>
    /// <param name="id">ID da pessoa a ser deletada</param>
    public async Task Delete(Guid id)
    {
      var result = await _dbContext.Pessoas.FindAsync(id);
      if (result != null)
      {
        _dbContext.Pessoas.Remove(result);
        await _dbContext.SaveChangesAsync();
      }
    }

    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// Utiliza AsNoTracking para melhorar performance em operações de leitura.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    public async Task<List<Pessoa>> GetAll()
    {
      return await _dbContext.Pessoas
        .AsNoTracking()
        .ToListAsync();
    }

    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da pessoa a ser obtida</param>
    /// <returns>A pessoa encontrada ou null se não existir</returns>
    public async Task<Pessoa?> GetById(Guid id)
    {
      return await _dbContext.Pessoas
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// </summary>
    /// <param name="pessoa">A pessoa com os dados atualizados</param>
    public async Task Update(Pessoa pessoa)
    {
      _dbContext.Pessoas.Update(pessoa);
      await _dbContext.SaveChangesAsync();
    }
  }
}
