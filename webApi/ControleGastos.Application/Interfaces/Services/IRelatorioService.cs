using ControleGastos.Application.DTOs.Relatorios;

namespace ControleGastos.Application.Interfaces.Services
{
  /// <summary>
  /// Interface para o serviço de relatórios.
  /// Define os métodos para gerar relatórios de totais de receitas, despesas e saldos.
  /// </summary>
  public interface IRelatorioService
  {
    /// <summary>
    /// Gera relatório com totais por pessoa.
    /// Retorna para cada pessoa: Total de Receitas, Total de Despesas e Saldo (Receita - Despesa).
    /// Também inclui totais gerais consolidados de todas as pessoas.
    /// </summary>
    /// <returns>Relatório contendo lista de totais por pessoa e totais gerais</returns>
    Task<RelatorioTotalsPorPessoaDto> GetTotalsPorPessoaAsync();

    /// <summary>
    /// Gera relatório com totais por categoria.
    /// Retorna para cada categoria: Total de Receitas, Total de Despesas e Saldo (Receita - Despesa).
    /// Também inclui totais gerais consolidados de todas as categorias.
    /// </summary>
    /// <returns>Relatório contendo lista de totais por categoria e totais gerais</returns>
    Task<RelatorioTotalsPorCategoriaDto> GetTotalsPorCategoriaAsync();
  }
}
