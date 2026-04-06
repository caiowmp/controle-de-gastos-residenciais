namespace ControleGastos.Application.Interfaces.Repositories.Transacao
{
  public interface ITransacaoReadOnly
  {
    Task<List<Domain.Entities.Transacao>> GetAll();
  }
}
