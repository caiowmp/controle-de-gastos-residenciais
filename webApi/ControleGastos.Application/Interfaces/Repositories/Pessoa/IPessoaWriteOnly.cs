namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  /// <summary>
  /// Interface para operações de escrita da entidade Pessoa.
  /// Define os métodos disponíveis para criar e deletar pessoas.
  /// </summary>
  public interface IPessoaWriteOnly
  {
    /// <summary>
    /// Adiciona uma nova pessoa ao banco de dados.
    /// Quando uma pessoa é adicionada, pode haver transações associadas.
    /// </summary>
    /// <param name="pessoa">A pessoa a ser adicionada</param>
    Task Add(Domain.Entities.Pessoa pessoa);

    /// <summary>
    /// Deleta uma pessoa do banco de dados.
    /// Quando uma pessoa é deletada, todas suas transações associadas serão removidas automaticamente (cascata).
    /// </summary>
    /// <param name="id">ID da pessoa a ser deletada</param>
    Task Delete(Guid id);
  }
}
