using ControleGastos.Application.DTOs.Transacao;
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
  /// Testes da regra de negócio crítica: menores de 18 anos não podem ter receitas
  /// </summary>
  public class TransacaoServiceMenorDeIdadeTests
  {
    private readonly Mock<ITransacaoReadOnly> _mockTransacaoRead;
    private readonly Mock<ITransacaoWriteOnly> _mockTransacaoWrite;
    private readonly Mock<IPessoaReadOnly> _mockPessoaRead;
    private readonly Mock<ICategoriaReadOnly> _mockCategoriaRead;
    private readonly TransacaoService _service;

    public TransacaoServiceMenorDeIdadeTests()
    {
      _mockTransacaoRead = new Mock<ITransacaoReadOnly>();
      _mockTransacaoWrite = new Mock<ITransacaoWriteOnly>();
      _mockPessoaRead = new Mock<IPessoaReadOnly>();
      _mockCategoriaRead = new Mock<ICategoriaReadOnly>();
      
      _service = new TransacaoService(
        _mockTransacaoRead.Object,
        _mockTransacaoWrite.Object,
        _mockPessoaRead.Object,
        _mockCategoriaRead.Object);
    }

    /// <summary>
    /// Teste: Menor de 18 anos tenta criar RECEITA → Deve ser bloqueado
    /// </summary>
    [Fact]
    public async Task CreateAsync_MenorDeIdadeComReceita_DeveThrowException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa 
      { 
        Id = pessoaId, 
        Nome = "João Silva", 
        Idade = 15  // MENOR DE IDADE
      };

      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Salário",
        Finalidade = FinalidadeCategoria.Receita
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Salário de férias",
        Valor = 500,
        Tipo = TipoTransacao.Receita,  // RECEITA
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("Menores de 18 anos", exception.Message);
      Assert.Contains("não podem ter transações de receita", exception.Message);
    }

    /// <summary>
    /// Teste: Menor de 18 anos cria DESPESA → Deve ser permitido
    /// </summary>
    [Fact]
    public async Task CreateAsync_MenorDeIdadeComDespesa_DeveSerPermitido()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa
      {
        Id = pessoaId,
        Nome = "João Silva",
        Idade = 15  // MENOR DE IDADE
      };

      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Alimentação",
        Finalidade = FinalidadeCategoria.Despesa
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Compra de alimentos",
        Valor = 50,
        Tipo = TipoTransacao.Despesa,  // DESPESA - OK!
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);
      _mockTransacaoWrite.Setup(x => x.Add(It.IsAny<Transacao>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.CreateAsync(createDto);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("João Silva", resultado.NomePessoa);
      _mockTransacaoWrite.Verify(x => x.Add(It.IsAny<Transacao>()), Times.Once);
    }

    /// <summary>
    /// Teste: Maior de 18 anos cria RECEITA → Deve ser permitido
    /// </summary>
    [Fact]
    public async Task CreateAsync_MaiorDeIdadeComReceita_DeveSerPermitido()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa
      {
        Id = pessoaId,
        Nome = "Maria Silva",
        Idade = 25  // MAIOR DE IDADE
      };

      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Salário",
        Finalidade = FinalidadeCategoria.Receita
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Salário mensal",
        Valor = 2000,
        Tipo = TipoTransacao.Receita,  // RECEITA - OK!
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);
      _mockTransacaoWrite.Setup(x => x.Add(It.IsAny<Transacao>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.CreateAsync(createDto);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("Maria Silva", resultado.NomePessoa);
      _mockTransacaoWrite.Verify(x => x.Add(It.IsAny<Transacao>()), Times.Once);
    }
  }

  /// <summary>
  /// Testes da regra de negócio crítica: categoria deve ser compatível com tipo de transação
  /// </summary>
  public class TransacaoServiceCategoriaCompatibilidadeTests
  {
    private readonly Mock<ITransacaoReadOnly> _mockTransacaoRead;
    private readonly Mock<ITransacaoWriteOnly> _mockTransacaoWrite;
    private readonly Mock<IPessoaReadOnly> _mockPessoaRead;
    private readonly Mock<ICategoriaReadOnly> _mockCategoriaRead;
    private readonly TransacaoService _service;

    public TransacaoServiceCategoriaCompatibilidadeTests()
    {
      _mockTransacaoRead = new Mock<ITransacaoReadOnly>();
      _mockTransacaoWrite = new Mock<ITransacaoWriteOnly>();
      _mockPessoaRead = new Mock<IPessoaReadOnly>();
      _mockCategoriaRead = new Mock<ICategoriaReadOnly>();
      
      _service = new TransacaoService(
        _mockTransacaoRead.Object,
        _mockTransacaoWrite.Object,
        _mockPessoaRead.Object,
        _mockCategoriaRead.Object);
    }

    /// <summary>
    /// Teste: Criar DESPESA com categoria RECEITA → Deve ser bloqueado
    /// </summary>
    [Fact]
    public async Task CreateAsync_DespesaComCategoriaReceita_DeveThrowException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };
      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Salário",
        Finalidade = FinalidadeCategoria.Receita  // SÓ PARA RECEITAS
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Compra no supermercado",
        Valor = 100,
        Tipo = TipoTransacao.Despesa,  // MAS QUEREMOS CRIAR DESPESA
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("não é permitida para despesas", exception.Message);
    }

    /// <summary>
    /// Teste: Criar RECEITA com categoria DESPESA → Deve ser bloqueado
    /// </summary>
    [Fact]
    public async Task CreateAsync_ReceitaComCategoriaDespesa_DeveThrowException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };
      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Alimentação",
        Finalidade = FinalidadeCategoria.Despesa  // SÓ PARA DESPESAS
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Salário",
        Valor = 3000,
        Tipo = TipoTransacao.Receita,  // MAS QUEREMOS CRIAR RECEITA
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("não é permitida para receitas", exception.Message);
    }

    /// <summary>
    /// Teste: Criar DESPESA com categoria AMBAS → Deve ser permitido
    /// </summary>
    [Fact]
    public async Task CreateAsync_DespesaComCategoriaAmbas_DeveSerPermitido()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };
      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Geral",
        Finalidade = FinalidadeCategoria.Ambas  // FUNCIONA PARA AMBAS
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Transferência",
        Valor = 500,
        Tipo = TipoTransacao.Despesa,
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);
      _mockTransacaoWrite.Setup(x => x.Add(It.IsAny<Transacao>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.CreateAsync(createDto);

      // Assert
      Assert.NotNull(resultado);
      _mockTransacaoWrite.Verify(x => x.Add(It.IsAny<Transacao>()), Times.Once);
    }

    /// <summary>
    /// Teste: Criar RECEITA com categoria AMBAS → Deve ser permitido
    /// </summary>
    [Fact]
    public async Task CreateAsync_ReceitaComCategoriaAmbas_DeveSerPermitido()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };
      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Geral",
        Finalidade = FinalidadeCategoria.Ambas  // FUNCIONA PARA AMBAS
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Salário",
        Valor = 3000,
        Tipo = TipoTransacao.Receita,
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);
      _mockTransacaoWrite.Setup(x => x.Add(It.IsAny<Transacao>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.CreateAsync(createDto);

      // Assert
      Assert.NotNull(resultado);
      _mockTransacaoWrite.Verify(x => x.Add(It.IsAny<Transacao>()), Times.Once);
    }
  }

  /// <summary>
  /// Testes de validações gerais de Transacao
  /// </summary>
  public class TransacaoServiceValidacoesGeralTests
  {
    private readonly Mock<ITransacaoReadOnly> _mockTransacaoRead;
    private readonly Mock<ITransacaoWriteOnly> _mockTransacaoWrite;
    private readonly Mock<IPessoaReadOnly> _mockPessoaRead;
    private readonly Mock<ICategoriaReadOnly> _mockCategoriaRead;
    private readonly TransacaoService _service;

    public TransacaoServiceValidacoesGeralTests()
    {
      _mockTransacaoRead = new Mock<ITransacaoReadOnly>();
      _mockTransacaoWrite = new Mock<ITransacaoWriteOnly>();
      _mockPessoaRead = new Mock<IPessoaReadOnly>();
      _mockCategoriaRead = new Mock<ICategoriaReadOnly>();
      
      _service = new TransacaoService(
        _mockTransacaoRead.Object,
        _mockTransacaoWrite.Object,
        _mockPessoaRead.Object,
        _mockCategoriaRead.Object);
    }

    /// <summary>
    /// Teste: Pessoa não existe → Deve lançar KeyNotFoundException
    /// </summary>
    [Fact]
    public async Task CreateAsync_PessoaNaoExiste_DeveThrowKeyNotFoundException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Test",
        Valor = 100,
        Tipo = TipoTransacao.Despesa,
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync((Pessoa?)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(
        () => _service.CreateAsync(createDto));
    }

    /// <summary>
    /// Teste: Categoria não existe → Deve lançar KeyNotFoundException
    /// </summary>
    [Fact]
    public async Task CreateAsync_CategoriaNaoExiste_DeveThrowKeyNotFoundException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Test",
        Valor = 100,
        Tipo = TipoTransacao.Despesa,
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync((Categoria?)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(
        () => _service.CreateAsync(createDto));
    }

    /// <summary>
    /// Teste: Valor <= 0 → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_ValorNegativoOuZero_DeveThrowArgumentException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var categoriaId = Guid.NewGuid();

      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };
      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Test",
        Finalidade = FinalidadeCategoria.Despesa
      };

      var createDto = new CreateTransacaoDto
      {
        Descricao = "Test",
        Valor = 0,  // VALOR INVÁLIDO
        Tipo = TipoTransacao.Despesa,
        PessoaId = pessoaId,
        CategoriaId = categoriaId
      };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("Valor deve ser maior que zero", exception.Message);
    }
  }
}
