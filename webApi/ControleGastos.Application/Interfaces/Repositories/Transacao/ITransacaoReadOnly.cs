namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  /// <summary>
  /// Interface para operações de leitura da entidade Transacao.
  /// Define os métodos disponíveis para consultar dados de transações.
  /// </summary>
  public interface ITransacaoReadOnly
  {
    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as transações</returns>
    Task<List<Domain.Entities.Transacao>> GetAll();

    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="pessoaId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    Task<List<Domain.Entities.Transacao>> GetByPessoaId(Guid pessoaId);

    /// <summary>
    /// Obtém uma transação específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da transação a ser obtida</param>
    /// <returns>A transação encontrada ou null se não existir</returns>
    Task<Domain.Entities.Transacao?> GetById(Guid id);
  }
}
