using ControleGastos.Domain.Enums;

namespace ControleGastos.Application.DTOs.Categoria
{
  public class CategoriaResponseDto
  {
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public FinalidadeCategoria Finalidade { get; set; }
  }
}
