namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  /// <summary>
  /// Interface para operações de leitura da entidade Categoria.
  /// Define os métodos disponíveis para consultar dados de categorias.
  /// </summary>
  public interface ICategoriaReadOnly
  {
    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    Task<List<Domain.Entities.Categoria>> GetAll();

    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da categoria a ser obtida</param>
    /// <returns>A categoria encontrada ou null se não existir</returns>
    Task<Domain.Entities.Categoria?> GetById(Guid id);
  }
}
