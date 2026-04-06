namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  public interface ICategoriaReadOnly
  {
    Task<List<Domain.Entities.Categoria>> GetAll();
  }
}
