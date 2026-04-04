namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  internal interface ITransacaoWriteOnly
  {
    Task Add(Domain.Entities.Transacao transacao);

    /// <summary>
    /// Essa função retorna TRUE se a DELETE foi feito com sucesso do contrário retorna FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
  }
}
