namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  /// <summary>
  /// Interface para operações de escrita da entidade Transacao.
  /// Define os métodos disponíveis para criar transações.
  /// </summary>
  public interface ITransacaoWriteOnly
  {
    /// <summary>
    /// Adiciona uma nova transação ao banco de dados.
    /// A transação sempre está associada a uma pessoa e uma categoria.
    /// </summary>
    /// <param name="transacao">A transação a ser adicionada</param>
    Task Add(Domain.Entities.Transacao transacao);
  }
}
