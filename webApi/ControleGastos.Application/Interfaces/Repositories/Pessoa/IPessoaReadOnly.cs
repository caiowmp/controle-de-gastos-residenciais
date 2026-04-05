namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  public interface IPessoaReadOnly
  {
    Task<List<Domain.Entities.Pessoa>> GetAll();
  }
}
