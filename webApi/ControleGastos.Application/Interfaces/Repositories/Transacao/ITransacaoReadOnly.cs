namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  internal interface ITransacaoReadOnly
  {
    Task<List<Domain.Entities.Transacao>> GetAll(Domain.Entities.Transacao transacao);
  }
}
