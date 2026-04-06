using ControleGastos.Application.DTOs.Relatorio;

namespace ControleGastos.Application.DTOs.Relatorios
{
  /// <summary>
  /// DTO contendo o relatório completo de totais por pessoa.
  /// Inclui uma lista com os totais de cada pessoa e os totais gerais consolidados.
  /// </summary>
  public class RelatorioTotalsPorPessoaDto
  {
    /// <summary>
    /// Lista com o resumo financeiro de cada pessoa.
    /// </summary>
    public List<TotalPorPessoaDto> Pessoas { get; set; } = new();

    /// <summary>
    /// Totais gerais consolidados de todas as pessoas.
    /// </summary>
    public TotalGeralDto TotaisGerais { get; set; } = new();
  }
}
