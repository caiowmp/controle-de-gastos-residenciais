namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  /// <summary>
  /// Interface para operações de atualização da entidade Pessoa.
  /// Define os métodos disponíveis para editar dados existentes de pessoas.
  /// </summary>
  public interface IPessoaUpdateOnly
  {
    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// </summary>
    /// <param name="pessoa">A pessoa com os dados atualizados</param>
    Task Update(Domain.Entities.Pessoa pessoa);
  }
}
