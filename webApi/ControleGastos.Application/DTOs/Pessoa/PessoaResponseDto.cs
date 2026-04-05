namespace ControleGastos.Application.DTOs.Pessoa
{
  public class PessoaResponseDto
  {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
  }
}
