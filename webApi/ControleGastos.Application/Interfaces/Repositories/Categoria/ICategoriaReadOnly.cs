namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  internal interface ICategoriaReadOnly
  {
    Task<List<Categoria>> GetAll(Categoria categoria);
  }
}
