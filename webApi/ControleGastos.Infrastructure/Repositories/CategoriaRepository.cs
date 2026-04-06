using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Domain.Entities;
using ControleGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories
{
  /// <summary>
  /// Repositório para operações de leitura e escrita da entidade Categoria.
  /// Gerencia as categorias de transações (Receita, Despesa, Ambas).
  /// </summary>
  internal class CategoriaRepository(ControleGastosDbContext _dbContext) : ICategoriaReadOnly, ICategoriaWriteOnly
  {
    /// <summary>
    /// Adiciona uma nova categoria ao banco de dados.
    /// </summary>
    /// <param name="categoria">A categoria a ser adicionada</param>
    public async Task Add(Categoria categoria)
    {
      await _dbContext.Categorias.AddAsync(categoria);
      await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// Utiliza AsNoTracking para melhorar performance em operações de leitura.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    public async Task<List<Categoria>> GetAll()
    {
      return await _dbContext.Categorias
        .AsNoTracking()
        .ToListAsync();
    }

    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da categoria a ser obtida</param>
    /// <returns>A categoria encontrada ou null se não existir</returns>
    public async Task<Categoria?> GetById(Guid id)
    {
      return await _dbContext.Categorias
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);
    }
  }
}
