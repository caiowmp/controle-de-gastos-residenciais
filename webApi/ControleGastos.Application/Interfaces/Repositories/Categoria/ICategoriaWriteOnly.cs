namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  /// <summary>
  /// Interface para operações de escrita da entidade Categoria.
  /// Define os métodos disponíveis para criar categorias.
  /// </summary>
  public interface ICategoriaWriteOnly
  {
    /// <summary>
    /// Adiciona uma nova categoria ao banco de dados.
    /// </summary>
    /// <param name="categoria">A categoria a ser adicionada</param>
    Task Add(Domain.Entities.Categoria categoria);
  }
}
