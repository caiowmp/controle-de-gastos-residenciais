namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  public interface ICategoriaWriteOnly
  {
    Task Add(Domain.Entities.Categoria categoira);
  }
}
