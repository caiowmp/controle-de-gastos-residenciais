using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Application.Services;
using ControleGastos.Domain.Entities;
using ControleGastos.Domain.Enums;
using Moq;

namespace ControleGastos.Tests.Services
{
  /// <summary>
  /// Testes do serviço RelatorioService
  /// Valida que os cálculos de receitas, despesas e saldos estão corretos
  /// </summary>
  public class RelatorioServiceTests
  {
    private readonly Mock<IPessoaReadOnly> _mockPessoaRead;
    private readonly Mock<ICategoriaReadOnly> _mockCategoriaRead;
    private readonly Mock<ITransacaoReadOnly> _mockTransacaoRead;
    private readonly RelatorioService _service;

    public RelatorioServiceTests()
    {
      _mockPessoaRead = new Mock<IPessoaReadOnly>();
      _mockCategoriaRead = new Mock<ICategoriaReadOnly>();
      _mockTransacaoRead = new Mock<ITransacaoReadOnly>();
      
      _service = new RelatorioService(
        _mockPessoaRead.Object,
        _mockCategoriaRead.Object,
        _mockTransacaoRead.Object);
    }

    /// <summary>
    /// Teste: Relatório por pessoa com dados de exemplo
    /// Valida cálculos de receita, despesa e saldo
    /// </summary>
    [Fact]
    public async Task GetTotalsPorPessoaAsync_ComTransacoes_DeveCalcularTotaisCorretamente()
    {
      // Arrange
      var pessoaId1 = Guid.NewGuid();
      var pessoaId2 = Guid.NewGuid();

      var pessoas = new List<Pessoa>
      {
        new() { Id = pessoaId1, Nome = "João", Idade = 30 },
        new() { Id = pessoaId2, Nome = "Maria", Idade = 25 }
      };

      var transacoes = new List<Transacao>
      {
        // João: 2 receitas (1000 + 500 = 1500), 2 despesas (300 + 200 = 500)
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId1, Tipo = TipoTransacao.Receita, Valor = 1000 },
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId1, Tipo = TipoTransacao.Receita, Valor = 500 },
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId1, Tipo = TipoTransacao.Despesa, Valor = 300 },
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId1, Tipo = TipoTransacao.Despesa, Valor = 200 },

        // Maria: 1 receita (2000), 1 despesa (150)
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId2, Tipo = TipoTransacao.Receita, Valor = 2000 },
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId2, Tipo = TipoTransacao.Despesa, Valor = 150 }
      };

      _mockPessoaRead.Setup(x => x.GetAll()).ReturnsAsync(pessoas);
      _mockTransacaoRead.Setup(x => x.GetAll()).ReturnsAsync(transacoes);

      // Act
      var resultado = await _service.GetTotalsPorPessoaAsync();

      // Assert - Validar dados
      Assert.NotNull(resultado);
      Assert.Equal(2, resultado.Pessoas.Count);

      // Validar João
      var joao = resultado.Pessoas.First(p => p.NomePessoa == "João");
      Assert.Equal(1500, joao.TotalReceitas);  // 1000 + 500
      Assert.Equal(500, joao.TotalDespesas);   // 300 + 200
      Assert.Equal(1000, joao.Saldo);          // 1500 - 500

      // Validar Maria
      var maria = resultado.Pessoas.First(p => p.NomePessoa == "Maria");
      Assert.Equal(2000, maria.TotalReceitas);
      Assert.Equal(150, maria.TotalDespesas);
      Assert.Equal(1850, maria.Saldo);  // 2000 - 150

      // Validar Totais Gerais
      Assert.Equal(3500, resultado.TotaisGerais.TotalReceitas);  // 1500 + 2000
      Assert.Equal(650, resultado.TotaisGerais.TotalDespesas);   // 500 + 150
      Assert.Equal(2850, resultado.TotaisGerais.Saldo);          // 3500 - 650
    }

    /// <summary>
    /// Teste: Relatório por pessoa sem transações
    /// Valida que pessoas sem transações têm totais zerados
    /// </summary>
    [Fact]
    public async Task GetTotalsPorPessoaAsync_PessoaSemTransacoes_DeveRetornarZero()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var pessoas = new List<Pessoa>
      {
        new() { Id = pessoaId, Nome = "João", Idade = 30 }
      };

      var transacoes = new List<Transacao>();  // SEM TRANSAÇÕES

      _mockPessoaRead.Setup(x => x.GetAll()).ReturnsAsync(pessoas);
      _mockTransacaoRead.Setup(x => x.GetAll()).ReturnsAsync(transacoes);

      // Act
      var resultado = await _service.GetTotalsPorPessoaAsync();

      // Assert
      Assert.NotNull(resultado);
      Assert.Single(resultado.Pessoas);
      
      var pessoa = resultado.Pessoas.First();
      Assert.Equal(0, pessoa.TotalReceitas);
      Assert.Equal(0, pessoa.TotalDespesas);
      Assert.Equal(0, pessoa.Saldo);

      Assert.Equal(0, resultado.TotaisGerais.TotalReceitas);
      Assert.Equal(0, resultado.TotaisGerais.TotalDespesas);
      Assert.Equal(0, resultado.TotaisGerais.Saldo);
    }

    /// <summary>
    /// Teste: Relatório por categoria com dados de exemplo
    /// Valida cálculos por categoria
    /// </summary>
    [Fact]
    public async Task GetTotalsPorCategoriaAsync_ComTransacoes_DeveCalcularTotaisCorretamente()
    {
      // Arrange
      var categoriaId1 = Guid.NewGuid();
      var categoriaId2 = Guid.NewGuid();

      var categorias = new List<Categoria>
      {
        new() { Id = categoriaId1, Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa },
        new() { Id = categoriaId2, Descricao = "Salário", Finalidade = FinalidadeCategoria.Receita }
      };

      var transacoes = new List<Transacao>
      {
        // Alimentação: 2 despesas (300 + 150 = 450)
        new() { Id = Guid.NewGuid(), CategoriaId = categoriaId1, Tipo = TipoTransacao.Despesa, Valor = 300 },
        new() { Id = Guid.NewGuid(), CategoriaId = categoriaId1, Tipo = TipoTransacao.Despesa, Valor = 150 },

        // Salário: 2 receitas (2000 + 1500 = 3500)
        new() { Id = Guid.NewGuid(), CategoriaId = categoriaId2, Tipo = TipoTransacao.Receita, Valor = 2000 },
        new() { Id = Guid.NewGuid(), CategoriaId = categoriaId2, Tipo = TipoTransacao.Receita, Valor = 1500 }
      };

      _mockCategoriaRead.Setup(x => x.GetAll()).ReturnsAsync(categorias);
      _mockTransacaoRead.Setup(x => x.GetAll()).ReturnsAsync(transacoes);

      // Act
      var resultado = await _service.GetTotalsPorCategoriaAsync();

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal(2, resultado.Categorias.Count);

      // Validar Alimentação
      var alimentacao = resultado.Categorias.First(c => c.Categoria == "Alimentação");
      Assert.Equal(0, alimentacao.TotalReceitas);    // Sem receitas
      Assert.Equal(450, alimentacao.TotalDespesas);  // 300 + 150
      Assert.Equal(-450, alimentacao.Saldo);         // 0 - 450

      // Validar Salário
      var salario = resultado.Categorias.First(c => c.Categoria == "Salário");
      Assert.Equal(3500, salario.TotalReceitas);     // 2000 + 1500
      Assert.Equal(0, salario.TotalDespesas);        // Sem despesas
      Assert.Equal(3500, salario.Saldo);             // 3500 - 0

      // Validar Totais Gerais
      Assert.Equal(3500, resultado.TotaisGerais.TotalReceitas);
      Assert.Equal(450, resultado.TotaisGerais.TotalDespesas);
      Assert.Equal(3050, resultado.TotaisGerais.Saldo);  // 3500 - 450
    }

    /// <summary>
    /// Teste: Validar que saldo pode ser negativo
    /// Quando despesas > receitas
    /// </summary>
    [Fact]
    public async Task GetTotalsPorPessoaAsync_DespesasMaioresQueReceitas_DeveSaldoNegativo()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var pessoas = new List<Pessoa>
      {
        new() { Id = pessoaId, Nome = "João", Idade = 30 }
      };

      var transacoes = new List<Transacao>
      {
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId, Tipo = TipoTransacao.Receita, Valor = 500 },
        new() { Id = Guid.NewGuid(), PessoaId = pessoaId, Tipo = TipoTransacao.Despesa, Valor = 1000 }
      };

      _mockPessoaRead.Setup(x => x.GetAll()).ReturnsAsync(pessoas);
      _mockTransacaoRead.Setup(x => x.GetAll()).ReturnsAsync(transacoes);

      // Act
      var resultado = await _service.GetTotalsPorPessoaAsync();

      // Assert
      var pessoa = resultado.Pessoas.First();
      Assert.Equal(500, pessoa.TotalReceitas);
      Assert.Equal(1000, pessoa.TotalDespesas);
      Assert.Equal(-500, pessoa.Saldo);  // NEGATIVO
    }
  }
}
