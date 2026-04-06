namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  public interface IPessoaWriteOnly
  {
    Task Add(Domain.Entities.Pessoa pessoa);

    /// <summary>
    /// Essa função retorna TRUE se a DELETE foi feito com sucesso do contrário retorna FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(Guid id);
  }
}
