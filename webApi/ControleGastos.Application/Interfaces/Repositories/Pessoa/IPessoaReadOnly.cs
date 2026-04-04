namespace ControleGastos.Application.Interfaces.Repositories.Pessoa
{
  internal interface IPessoaReadOnly
  {
    Task<List<Pessoa>> GetAll(Pessoa pesosa);
  }
}
