using ControleGastos.Application.DTOs.Transacao;

namespace ControleGastos.Application.Interfaces.Services
{
  /// <summary>
  /// Interface para o serviço de gerenciamento de Transações.
  /// Define os métodos para operações de criação e listagem de transações.
  /// Implementa validações de regras de negócio específicas do domínio.
  /// </summary>
  public interface ITransacaoService
  {
    /// <summary>
    /// Cria uma nova transação no sistema.
    /// Implementa validações críticas:
    /// - Menores de 18 anos: apenas despesas são permitidas
    /// - A categoria deve ser compatível com o tipo de transação (Receita/Despesa)
    /// - Valores devem ser positivos
    /// - Pessoa e categoria devem existir
    /// </summary>
    /// <param name="createTransacaoDto">Dados da transação a ser criada</param>
    /// <returns>Dados da transação criada</returns>
    Task<TransacaoResponseDto> CreateAsync(CreateTransacaoDto createTransacaoDto);

    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// Inclui informações da pessoa e categoria associadas.
    /// </summary>
    /// <returns>Lista de todas as transações</returns>
    Task<List<TransacaoResponseDto>> GetAllAsync();

    /// <summary>
    /// Obtém uma transação específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da transação</param>
    /// <returns>Dados da transação encontrada</returns>
    Task<TransacaoResponseDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="pessoaId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    Task<List<TransacaoResponseDto>> GetByPessoaIdAsync(Guid pessoaId);
  }
}
