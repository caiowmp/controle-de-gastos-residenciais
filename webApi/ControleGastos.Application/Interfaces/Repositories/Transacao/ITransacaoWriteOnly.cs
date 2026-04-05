namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  public interface ITransacaoWriteOnly
  {
    Task Add(Domain.Entities.Transacao transacao);
  }
}
