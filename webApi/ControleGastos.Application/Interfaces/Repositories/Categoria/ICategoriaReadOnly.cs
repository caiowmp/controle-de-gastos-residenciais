namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  internal interface ICategoriaReadOnly
  {
    Task<List<Domain.Entities.Categoria>> GetAll(Domain.Entities.Categoria categoria);
  }
}
