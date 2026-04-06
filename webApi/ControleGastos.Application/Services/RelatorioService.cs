using ControleGastos.Application.DTOs.Relatorios;
using ControleGastos.Application.DTOs.Relatorio;
using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Application.Interfaces.Services;
using ControleGastos.Domain.Enums;

namespace ControleGastos.Application.Services
{
  /// <summary>
  /// Serviço para geração de relatórios financeiros.
  /// Calcula totais de receitas, despesas e saldos por pessoa e por categoria.
  /// Fornece visão consolidada dos dados financeiros do sistema.
  /// </summary>
  public class RelatorioService(
    IPessoaReadOnly pessoaReadOnly,
    ICategoriaReadOnly categoriaReadOnly,
    ITransacaoReadOnly transacaoReadOnly) : IRelatorioService
  {
    /// <summary>
    /// Gera relatório com totais de receitas, despesas e saldos por pessoa.
    /// 
    /// O relatório inclui:
    /// 1. Lista de cada pessoa com seus totais:
    ///    - Total de Receitas (soma de todas as transações tipo Receita)
    ///    - Total de Despesas (soma de todas as transações tipo Despesa)
    ///    - Saldo (Receita - Despesa)
    /// 2. Totais gerais consolidados de todas as pessoas
    /// </summary>
    /// <returns>Relatório com totais por pessoa e totais gerais</returns>
    public async Task<RelatorioTotalsPorPessoaDto> GetTotalsPorPessoaAsync()
    {
      var pessoas = await pessoaReadOnly.GetAll();
      var transacoes = await transacaoReadOnly.GetAll();

      var totalsPorPessoa = new List<TotalPorPessoaDto>();
      decimal totalReceitasGeral = 0;
      decimal totalDespesasGeral = 0;

      foreach (var pessoa in pessoas)
      {
        // Calcular totais para esta pessoa
        var transacoesPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id).ToList();

        decimal totalReceitas = transacoesPessoa
          .Where(t => t.Tipo == TipoTransacao.Receita)
          .Sum(t => t.Valor);

        decimal totalDespesas = transacoesPessoa
          .Where(t => t.Tipo == TipoTransacao.Despesa)
          .Sum(t => t.Valor);

        decimal saldo = totalReceitas - totalDespesas;

        totalsPorPessoa.Add(new TotalPorPessoaDto
        {
          NomePessoa = pessoa.Nome,
          TotalReceitas = totalReceitas,
          TotalDespesas = totalDespesas,
          Saldo = saldo
        });

        // Acumular totais gerais
        totalReceitasGeral += totalReceitas;
        totalDespesasGeral += totalDespesas;
      }

      return new RelatorioTotalsPorPessoaDto
      {
        Pessoas = totalsPorPessoa,
        TotaisGerais = new TotalGeralDto
        {
          TotalReceitas = totalReceitasGeral,
          TotalDespesas = totalDespesasGeral,
          Saldo = totalReceitasGeral - totalDespesasGeral
        }
      };
    }

    /// <summary>
    /// Gera relatório com totais de receitas, despesas e saldos por categoria.
    /// 
    /// O relatório inclui:
    /// 1. Lista de cada categoria com seus totais:
    ///    - Total de Receitas (soma de todas as transações tipo Receita)
    ///    - Total de Despesas (soma de todas as transações tipo Despesa)
    ///    - Saldo (Receita - Despesa)
    /// 2. Totais gerais consolidados de todas as categorias
    /// </summary>
    /// <returns>Relatório com totais por categoria e totais gerais</returns>
    public async Task<RelatorioTotalsPorCategoriaDto> GetTotalsPorCategoriaAsync()
    {
      var categorias = await categoriaReadOnly.GetAll();
      var transacoes = await transacaoReadOnly.GetAll();

      var totalsPorCategoria = new List<TotalPorCategoriaDto>();
      decimal totalReceitasGeral = 0;
      decimal totalDespesasGeral = 0;

      foreach (var categoria in categorias)
      {
        // Calcular totais para esta categoria
        var transacoesCategoria = transacoes.Where(t => t.CategoriaId == categoria.Id).ToList();

        decimal totalReceitas = transacoesCategoria
          .Where(t => t.Tipo == TipoTransacao.Receita)
          .Sum(t => t.Valor);

        decimal totalDespesas = transacoesCategoria
          .Where(t => t.Tipo == TipoTransacao.Despesa)
          .Sum(t => t.Valor);

        decimal saldo = totalReceitas - totalDespesas;

        totalsPorCategoria.Add(new TotalPorCategoriaDto
        {
          Categoria = categoria.Descricao,
          TotalReceitas = totalReceitas,
          TotalDespesas = totalDespesas,
          Saldo = saldo
        });

        // Acumular totais gerais
        totalReceitasGeral += totalReceitas;
        totalDespesasGeral += totalDespesas;
      }

      return new RelatorioTotalsPorCategoriaDto
      {
        Categorias = totalsPorCategoria,
        TotaisGerais = new TotalGeralDto
        {
          TotalReceitas = totalReceitasGeral,
          TotalDespesas = totalDespesasGeral,
          Saldo = totalReceitasGeral - totalDespesasGeral
        }
      };
    }
  }
}
