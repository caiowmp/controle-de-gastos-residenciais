using ControleGastos.Application.DTOs.Transacao;
using ControleGastos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers
{
  /// <summary>
  /// Controller para gerenciamento de Transações.
  /// Fornece endpoints para criação e listagem de transações financeiras.
  /// Implementa validações críticas de regras de negócio no serviço.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class TransacaoController(ITransacaoService transacaoService) : ControllerBase
  {
    /// <summary>
    /// Cria uma nova transação no sistema com validações críticas.
    /// 
    /// REGRAS DE NEGÓCIO VALIDADAS:
    /// 1. Menores de 18 anos: Apenas DESPESAS são permitidas
    /// 2. Compatibilidade de categoria:
    ///    - Despesa: Categoria deve ter Finalidade = Despesa ou Ambas
    ///    - Receita: Categoria deve ter Finalidade = Receita ou Ambas
    /// 3. Valores devem ser positivos
    /// 4. Pessoa e categoria devem existir
    /// </summary>
    /// <param name="createTransacaoDto">Dados da transação a ser criada</param>
    /// <returns>Dados da transação criada com status 201 Created</returns>
    /// <response code="201">Transação criada com sucesso</response>
    /// <response code="400">Dados inválidos ou regra de negócio violada</response>
    /// <response code="404">Pessoa ou categoria não encontrada</response>
    [HttpPost]
    public async Task<ActionResult<TransacaoResponseDto>> Create([FromBody] CreateTransacaoDto createTransacaoDto)
    {
      try
      {
        var resultado = await transacaoService.CreateAsync(createTransacaoDto);
        return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// Inclui informações de pessoa e categoria associadas.
    /// </summary>
    /// <returns>Lista de todas as transações</returns>
    /// <response code="200">Lista de transações retornada com sucesso</response>
    [HttpGet]
    public async Task<ActionResult<List<TransacaoResponseDto>>> GetAll()
    {
      var resultado = await transacaoService.GetAllAsync();
      return Ok(resultado);
    }

    /// <summary>
    /// Obtém uma transação específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da transação</param>
    /// <returns>Dados da transação encontrada</returns>
    /// <response code="200">Transação encontrada</response>
    /// <response code="404">Transação não encontrada</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<TransacaoResponseDto>> GetById([FromRoute] Guid id)
    {
      try
      {
        var resultado = await transacaoService.GetByIdAsync(id);
        return Ok(resultado);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// Útil para visualizar histórico financeiro de uma pessoa.
    /// </summary>
    /// <param name="pessoaId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    /// <response code="200">Lista de transações retornada com sucesso</response>
    /// <response code="404">Pessoa não encontrada</response>
    [HttpGet("pessoa/{pessoaId}")]
    public async Task<ActionResult<List<TransacaoResponseDto>>> GetByPessoaId([FromRoute] Guid pessoaId)
    {
      try
      {
        var resultado = await transacaoService.GetByPessoaIdAsync(pessoaId);
        return Ok(resultado);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }
  }
}
