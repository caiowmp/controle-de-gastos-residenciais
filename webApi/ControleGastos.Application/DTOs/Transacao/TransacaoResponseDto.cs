using ControleGastos.Domain.Enums;

namespace ControleGastos.Application.DTOs.Transacao
{
  public class TransacaoResponseDto
  {
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }

    public string NomePessoa { get; set; } = string.Empty;
    public string CategoriaDescricao { get; set; } = string.Empty;
  }
}
