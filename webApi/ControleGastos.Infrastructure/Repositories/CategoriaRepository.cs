using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Domain.Entities;
using ControleGastos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Repositories
{
  internal class CategoriaRepository(ControleGastosDbContext _dbContext) : ICategoriaReadOnly, ICategoriaWriteOnly
  {
    public async Task Add(Categoria categoira)
    {
      await _dbContext.Categorias.AddAsync(categoira);
    }

    public async Task<List<Categoria>> GetAll()
    {
      return await _dbContext.Categorias
        .AsNoTracking()
        .ToListAsync();
    }
  }
}
