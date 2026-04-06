using ControleGastos.Application.DTOs.Pessoa;

namespace ControleGastos.Application.Interfaces.Services
{
  /// <summary>
  /// Interface para o serviço de gerenciamento de Pessoas.
  /// Define os métodos para CRUD e operações de negócio relacionadas a pessoas.
  /// </summary>
  public interface IPessoaService
  {
    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// Valida os dados de entrada conforme as regras de negócio.
    /// </summary>
    /// <param name="createPessoaDto">Dados da pessoa a ser criada</param>
    /// <returns>Dados da pessoa criada</returns>
    Task<PessoaResponseDto> CreateAsync(CreatePessoaDto createPessoaDto);

    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da pessoa</param>
    /// <returns>Dados da pessoa encontrada</returns>
    Task<PessoaResponseDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    Task<List<PessoaResponseDto>> GetAllAsync();

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// Valida os dados conforme as regras de negócio.
    /// </summary>
    /// <param name="id">ID da pessoa a ser atualizada</param>
    /// <param name="updatePessoaDto">Novos dados da pessoa</param>
    /// <returns>Dados da pessoa atualizada</returns>
    Task<PessoaResponseDto> UpdateAsync(Guid id, UpdatePessoaDto updatePessoaDto);

    /// <summary>
    /// Deleta uma pessoa e todas as suas transações associadas.
    /// Importante: A deleção é em cascata, removendo também todas as transações da pessoa.
    /// </summary>
    /// <param name="id">ID da pessoa a ser deletada</param>
    Task DeleteAsync(Guid id);
  }
}
