using ControleGastos.Application.DTOs.Categoria;

namespace ControleGastos.Application.Interfaces.Services
{
  /// <summary>
  /// Interface para o serviço de gerenciamento de Categorias.
  /// Define os métodos para operações de criação e listagem de categorias.
  /// </summary>
  public interface ICategoriaService
  {
    /// <summary>
    /// Cria uma nova categoria no sistema.
    /// Valida os dados de entrada conforme as regras de negócio.
    /// </summary>
    /// <param name="createCategoriaDto">Dados da categoria a ser criada</param>
    /// <returns>Dados da categoria criada</returns>
    Task<CategoriaResponseDto> CreateAsync(CreateCategoriaDto createCategoriaDto);

    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    Task<List<CategoriaResponseDto>> GetAllAsync();

    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Dados da categoria encontrada</returns>
    Task<CategoriaResponseDto> GetByIdAsync(Guid id);
  }
}
