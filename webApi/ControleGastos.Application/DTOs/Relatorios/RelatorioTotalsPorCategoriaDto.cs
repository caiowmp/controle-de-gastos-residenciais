using ControleGastos.Application.DTOs.Relatorio;

namespace ControleGastos.Application.DTOs.Relatorios
{
  /// <summary>
  /// DTO contendo o relatório completo de totais por categoria.
  /// Inclui uma lista com os totais de cada categoria e os totais gerais consolidados.
  /// </summary>
  public class RelatorioTotalsPorCategoriaDto
  {
    /// <summary>
    /// Lista com o resumo financeiro de cada categoria.
    /// </summary>
    public List<TotalPorCategoriaDto> Categorias { get; set; } = new();

    /// <summary>
    /// Totais gerais consolidados de todas as categorias.
    /// </summary>
    public TotalGeralDto TotaisGerais { get; set; } = new();
  }
}
