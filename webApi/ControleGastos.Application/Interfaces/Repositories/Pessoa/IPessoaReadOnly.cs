namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  /// <summary>
  /// Interface para operações de leitura da entidade Pessoa.
  /// Define os métodos disponíveis para consultar dados de pessoas.
  /// </summary>
  public interface IPessoaReadOnly
  {
    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    Task<List<Domain.Entities.Pessoa>> GetAll();

    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da pessoa a ser obtida</param>
    /// <returns>A pessoa encontrada ou null se não existir</returns>
    Task<Domain.Entities.Pessoa?> GetById(Guid id);
  }
}
