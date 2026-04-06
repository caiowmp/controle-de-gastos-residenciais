using ControleGastos.Application.DTOs.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Services;
using ControleGastos.Domain.Entities;
using ControleGastos.Domain.Enums;
using Moq;

namespace ControleGastos.Tests.Services
{
  /// <summary>
  /// Testes do serviço CategoriaService
  /// </summary>
  public class CategoriaServiceTests
  {
    private readonly Mock<ICategoriaReadOnly> _mockCategoriaRead;
    private readonly Mock<ICategoriaWriteOnly> _mockCategoriaWrite;
    private readonly CategoriaService _service;

    public CategoriaServiceTests()
    {
      _mockCategoriaRead = new Mock<ICategoriaReadOnly>();
      _mockCategoriaWrite = new Mock<ICategoriaWriteOnly>();
      
      _service = new CategoriaService(
        _mockCategoriaRead.Object,
        _mockCategoriaWrite.Object);
    }

    /// <summary>
    /// Teste: Criar categoria com dados válidos → Deve ser criada
    /// </summary>
    [Fact]
    public async Task CreateAsync_DadosValidos_DeveCriarCategoria()
    {
      // Arrange
      var createDto = new CreateCategoriaDto
      {
        Descricao = "Alimentação",
        Finalidade = FinalidadeCategoria.Despesa
      };

      _mockCategoriaWrite.Setup(x => x.Add(It.IsAny<Categoria>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.CreateAsync(createDto);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("Alimentação", resultado.Descricao);
      Assert.Equal(FinalidadeCategoria.Despesa, resultado.Finalidade);
      _mockCategoriaWrite.Verify(x => x.Add(It.IsAny<Categoria>()), Times.Once);
    }

    /// <summary>
    /// Teste: Criar categoria com descrição vazia → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_DescricaoVazia_DeveThrowArgumentException()
    {
      // Arrange
      var createDto = new CreateCategoriaDto
      {
        Descricao = "",
        Finalidade = FinalidadeCategoria.Despesa
      };

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("Descrição é obrigatória", exception.Message);
    }

    /// <summary>
    /// Teste: Criar categoria com descrição > 400 caracteres → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_DescricaoComMuitosCaracteres_DeveThrowArgumentException()
    {
      // Arrange
      var createDto = new CreateCategoriaDto
      {
        Descricao = new string('A', 401),  // 401 caracteres
        Finalidade = FinalidadeCategoria.Despesa
      };

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("400 caracteres", exception.Message);
    }

    /// <summary>
    /// Teste: Obter categoria existente → Deve retornar dados
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_CategoriaExistente_DeveRetornarDados()
    {
      // Arrange
      var categoriaId = Guid.NewGuid();
      var categoria = new Categoria
      {
        Id = categoriaId,
        Descricao = "Alimentação",
        Finalidade = FinalidadeCategoria.Despesa
      };

      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync(categoria);

      // Act
      var resultado = await _service.GetByIdAsync(categoriaId);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("Alimentação", resultado.Descricao);
    }

    /// <summary>
    /// Teste: Obter categoria inexistente → Deve lançar KeyNotFoundException
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_CategoriaInexistente_DeveThrowKeyNotFoundException()
    {
      // Arrange
      var categoriaId = Guid.NewGuid();
      _mockCategoriaRead.Setup(x => x.GetById(categoriaId)).ReturnsAsync((Categoria?)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(
        () => _service.GetByIdAsync(categoriaId));
    }

    /// <summary>
    /// Teste: Listar todas as categorias → Deve retornar lista
    /// </summary>
    [Fact]
    public async Task GetAllAsync_CategoriasCadastradas_DeveRetornarLista()
    {
      // Arrange
      var categorias = new List<Categoria>
      {
        new() { Id = Guid.NewGuid(), Descricao = "Alimentação", Finalidade = FinalidadeCategoria.Despesa },
        new() { Id = Guid.NewGuid(), Descricao = "Salário", Finalidade = FinalidadeCategoria.Receita }
      };

      _mockCategoriaRead.Setup(x => x.GetAll()).ReturnsAsync(categorias);

      // Act
      var resultado = await _service.GetAllAsync();

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal(2, resultado.Count);
      Assert.Equal("Alimentação", resultado[0].Descricao);
      Assert.Equal("Salário", resultado[1].Descricao);
    }
  }
}
