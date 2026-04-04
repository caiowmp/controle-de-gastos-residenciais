namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  internal interface IPessoaReadOnly
  {
    Task<List<Domain.Entities.Pessoa>> GetAll(Domain.Entities.Pessoa pesosa);
  }
}
