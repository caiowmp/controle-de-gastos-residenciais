namespace ControleGastos.Application.Interfaces.Repositories.Categoria
{
  internal interface ICategoriaWriteOnly
  {
    Task Add(Categoria categoira);

    /// <summary>
    /// Essa função retorna TRUE se a DELETE foi feito com sucesso do contrário retorna FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
  }
}
