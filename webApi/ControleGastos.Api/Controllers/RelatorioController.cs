using ControleGastos.Application.DTOs.Relatorios;
using ControleGastos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers
{
  /// <summary>
  /// Controller para consulta de relatórios financeiros.
  /// Fornece endpoints para visualizar totalizações de receitas, despesas e saldos
  /// por pessoa e por categoria.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class RelatorioController(IRelatorioService relatorioService) : ControllerBase
  {
    /// <summary>
    /// Gera relatório com totais de receitas, despesas e saldos por pessoa.
    /// 
    /// O relatório inclui:
    /// 1. Uma linha para cada pessoa cadastrada com seus totais
    /// 2. Totais gerais consolidados de todas as pessoas
    /// 
    /// Cálculos:
    /// - Total Receitas: Soma de todas as transações tipo Receita
    /// - Total Despesas: Soma de todas as transações tipo Despesa
    /// - Saldo: Total Receitas - Total Despesas
    /// </summary>
    /// <returns>Relatório com totais por pessoa e totais gerais</returns>
    /// <response code="200">Relatório gerado com sucesso</response>
    [HttpGet("totais-por-pessoa")]
    public async Task<ActionResult<RelatorioTotalsPorPessoaDto>> GetTotalsPorPessoa()
    {
      var resultado = await relatorioService.GetTotalsPorPessoaAsync();
      return Ok(resultado);
    }

    /// <summary>
    /// Gera relatório com totais de receitas, despesas e saldos por categoria.
    /// 
    /// O relatório inclui:
    /// 1. Uma linha para cada categoria cadastrada com seus totais
    /// 2. Totais gerais consolidados de todas as categorias
    /// 
    /// Cálculos:
    /// - Total Receitas: Soma de todas as transações tipo Receita
    /// - Total Despesas: Soma de todas as transações tipo Despesa
    /// - Saldo: Total Receitas - Total Despesas
    /// </summary>
    /// <returns>Relatório com totais por categoria e totais gerais</returns>
    /// <response code="200">Relatório gerado com sucesso</response>
    [HttpGet("totais-por-categoria")]
    public async Task<ActionResult<RelatorioTotalsPorCategoriaDto>> GetTotalsPorCategoria()
    {
      var resultado = await relatorioService.GetTotalsPorCategoriaAsync();
      return Ok(resultado);
    }
  }
}
