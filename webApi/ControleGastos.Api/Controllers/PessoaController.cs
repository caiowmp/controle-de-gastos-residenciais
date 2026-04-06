using ControleGastos.Application.DTOs.Pessoa;
using ControleGastos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers
{
  /// <summary>
  /// Controller para gerenciamento de Pessoas.
  /// Fornece endpoints para CRUD (Create, Read, Update, Delete) de pessoas.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class PessoaController(IPessoaService pessoaService) : ControllerBase
  {
    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// </summary>
    /// <param name="createPessoaDto">Dados da pessoa a ser criada</param>
    /// <returns>Dados da pessoa criada com status 201 Created</returns>
    /// <response code="201">Pessoa criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    public async Task<ActionResult<PessoaResponseDto>> Create([FromBody] CreatePessoaDto createPessoaDto)
    {
      try
      {
        var resultado = await pessoaService.CreateAsync(createPessoaDto);
        return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da pessoa</param>
    /// <returns>Dados da pessoa encontrada</returns>
    /// <response code="200">Pessoa encontrada</response>
    /// <response code="404">Pessoa não encontrada</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<PessoaResponseDto>> GetById([FromRoute] Guid id)
    {
      try
      {
        var resultado = await pessoaService.GetByIdAsync(id);
        return Ok(resultado);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    /// <response code="200">Lista de pessoas retornada com sucesso</response>
    [HttpGet]
    public async Task<ActionResult<List<PessoaResponseDto>>> GetAll()
    {
      var resultado = await pessoaService.GetAllAsync();
      return Ok(resultado);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// </summary>
    /// <param name="id">ID da pessoa a ser atualizada</param>
    /// <param name="updatePessoaDto">Novos dados da pessoa</param>
    /// <returns>Dados da pessoa atualizada</returns>
    /// <response code="200">Pessoa atualizada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Pessoa não encontrada</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<PessoaResponseDto>> Update([FromRoute] Guid id, [FromBody] UpdatePessoaDto updatePessoaDto)
    {
      try
      {
        var resultado = await pessoaService.UpdateAsync(id, updatePessoaDto);
        return Ok(resultado);
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
    /// Deleta uma pessoa e todas as suas transações associadas.
    /// AVISO: Esta operação é irreversível e remove também todas as transações da pessoa.
    /// </summary>
    /// <param name="id">ID da pessoa a ser deletada</param>
    /// <returns>No content</returns>
    /// <response code="204">Pessoa deletada com sucesso</response>
    /// <response code="404">Pessoa não encontrada</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
      try
      {
        await pessoaService.DeleteAsync(id);
        return NoContent();
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }
  }
}
