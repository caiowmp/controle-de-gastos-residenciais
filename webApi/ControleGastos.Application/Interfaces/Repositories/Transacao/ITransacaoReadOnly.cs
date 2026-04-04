namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  internal interface ITransacaoReadOnly
  {
    Task<List<Transacao>> GetAll(Transacao transacao);
  }
}
