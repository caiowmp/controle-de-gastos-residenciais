namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  internal interface IPessoaWriteOnly
  {
    Task Add(Pessoa pessoa);

    /// <summary>
    /// Essa função retorna TRUE se a DELETE foi feito com sucesso do contrário retorna FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
  }
}
