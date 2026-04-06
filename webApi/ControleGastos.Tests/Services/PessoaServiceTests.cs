using ControleGastos.Application.DTOs.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Services;
using ControleGastos.Domain.Entities;
using Moq;

namespace ControleGastos.Tests.Services
{
  /// <summary>
  /// Testes do serviço PessoaService
  /// </summary>
  public class PessoaServiceTests
  {
    private readonly Mock<IPessoaReadOnly> _mockPessoaRead;
    private readonly Mock<IPessoaWriteOnly> _mockPessoaWrite;
    private readonly Mock<IPessoaUpdateOnly> _mockPessoaUpdate;
    private readonly PessoaService _service;

    public PessoaServiceTests()
    {
      _mockPessoaRead = new Mock<IPessoaReadOnly>();
      _mockPessoaWrite = new Mock<IPessoaWriteOnly>();
      _mockPessoaUpdate = new Mock<IPessoaUpdateOnly>();
      
      _service = new PessoaService(
        _mockPessoaRead.Object,
        _mockPessoaWrite.Object,
        _mockPessoaUpdate.Object);
    }

    /// <summary>
    /// Teste: Criar pessoa com dados válidos → Deve ser criada
    /// </summary>
    [Fact]
    public async Task CreateAsync_DadosValidos_DeveCriarPessoa()
    {
      // Arrange
      var createDto = new CreatePessoaDto
      {
        Nome = "João Silva",
        Idade = 30
      };

      _mockPessoaWrite.Setup(x => x.Add(It.IsAny<Pessoa>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.CreateAsync(createDto);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("João Silva", resultado.Nome);
      Assert.Equal(30, resultado.Idade);
      _mockPessoaWrite.Verify(x => x.Add(It.IsAny<Pessoa>()), Times.Once);
    }

    /// <summary>
    /// Teste: Criar pessoa com nome vazio → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_NomeVazio_DeveThrowArgumentException()
    {
      // Arrange
      var createDto = new CreatePessoaDto
      {
        Nome = "",
        Idade = 30
      };

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("Nome é obrigatório", exception.Message);
    }

    /// <summary>
    /// Teste: Criar pessoa com nome > 200 caracteres → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_NomeComMuitosCaracteres_DeveThrowArgumentException()
    {
      // Arrange
      var createDto = new CreatePessoaDto
      {
        Nome = new string('A', 201),  // 201 caracteres
        Idade = 30
      };

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("200 caracteres", exception.Message);
    }

    /// <summary>
    /// Teste: Criar pessoa com idade inválida (negativa) → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_IdadeNegativa_DeveThrowArgumentException()
    {
      // Arrange
      var createDto = new CreatePessoaDto
      {
        Nome = "João",
        Idade = -5
      };

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("idade deve estar entre 0 e 150", exception.Message.ToLower());
    }

    /// <summary>
    /// Teste: Criar pessoa com idade > 150 → Deve lançar ArgumentException
    /// </summary>
    [Fact]
    public async Task CreateAsync_IdadeAcimaDoLimite_DeveThrowArgumentException()
    {
      // Arrange
      var createDto = new CreatePessoaDto
      {
        Nome = "João",
        Idade = 151
      };

      // Act & Assert
      var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateAsync(createDto));

      Assert.Contains("idade deve estar entre 0 e 150", exception.Message.ToLower());
    }

    /// <summary>
    /// Teste: Obter pessoa existente → Deve retornar dados
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_PessoaExistente_DeveRetornarDados()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);

      // Act
      var resultado = await _service.GetByIdAsync(pessoaId);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("João", resultado.Nome);
      Assert.Equal(30, resultado.Idade);
    }

    /// <summary>
    /// Teste: Obter pessoa inexistente → Deve lançar KeyNotFoundException
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_PessoaInexistente_DeveThrowKeyNotFoundException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync((Pessoa?)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(
        () => _service.GetByIdAsync(pessoaId));
    }

    /// <summary>
    /// Teste: Atualizar pessoa com dados válidos → Deve atualizar
    /// </summary>
    [Fact]
    public async Task UpdateAsync_DadosValidos_DeveAtualizarPessoa()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };
      var updateDto = new UpdatePessoaDto { Nome = "João Silva", Idade = 31 };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockPessoaUpdate.Setup(x => x.Update(It.IsAny<Pessoa>())).Returns(Task.CompletedTask);

      // Act
      var resultado = await _service.UpdateAsync(pessoaId, updateDto);

      // Assert
      Assert.NotNull(resultado);
      Assert.Equal("João Silva", resultado.Nome);
      Assert.Equal(31, resultado.Idade);
      _mockPessoaUpdate.Verify(x => x.Update(It.IsAny<Pessoa>()), Times.Once);
    }

    /// <summary>
    /// Teste: Deletar pessoa existente → Deve deletar
    /// </summary>
    [Fact]
    public async Task DeleteAsync_PessoaExistente_DeveDeletarPessoa()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      var pessoa = new Pessoa { Id = pessoaId, Nome = "João", Idade = 30 };

      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync(pessoa);
      _mockPessoaWrite.Setup(x => x.Delete(pessoaId)).Returns(Task.CompletedTask);

      // Act
      await _service.DeleteAsync(pessoaId);

      // Assert
      _mockPessoaWrite.Verify(x => x.Delete(pessoaId), Times.Once);
    }

    /// <summary>
    /// Teste: Deletar pessoa inexistente → Deve lançar KeyNotFoundException
    /// </summary>
    [Fact]
    public async Task DeleteAsync_PessoaInexistente_DeveThrowKeyNotFoundException()
    {
      // Arrange
      var pessoaId = Guid.NewGuid();
      _mockPessoaRead.Setup(x => x.GetById(pessoaId)).ReturnsAsync((Pessoa?)null);

      // Act & Assert
      await Assert.ThrowsAsync<KeyNotFoundException>(
        () => _service.DeleteAsync(pessoaId));
    }
  }
}
