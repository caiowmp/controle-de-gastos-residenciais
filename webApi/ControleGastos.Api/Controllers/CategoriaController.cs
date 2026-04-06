using ControleGastos.Application.DTOs.Categoria;
using ControleGastos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.Api.Controllers
{
  /// <summary>
  /// Controller para gerenciamento de Categorias.
  /// Fornece endpoints para criação e listagem de categorias de transações.
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  public class CategoriaController(ICategoriaService categoriaService) : ControllerBase
  {
    /// <summary>
    /// Cria uma nova categoria no sistema.
    /// As categorias definem se uma transação é Receita, Despesa ou Ambas.
    /// </summary>
    /// <param name="createCategoriaDto">Dados da categoria a ser criada</param>
    /// <returns>Dados da categoria criada com status 201 Created</returns>
    /// <response code="201">Categoria criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    public async Task<ActionResult<CategoriaResponseDto>> Create([FromBody] CreateCategoriaDto createCategoriaDto)
    {
      try
      {
        var resultado = await categoriaService.CreateAsync(createCategoriaDto);
        return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    /// <response code="200">Lista de categorias retornada com sucesso</response>
    [HttpGet]
    public async Task<ActionResult<List<CategoriaResponseDto>>> GetAll()
    {
      var resultado = await categoriaService.GetAllAsync();
      return Ok(resultado);
    }

    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Dados da categoria encontrada</returns>
    /// <response code="200">Categoria encontrada</response>
    /// <response code="404">Categoria não encontrada</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaResponseDto>> GetById([FromRoute] Guid id)
    {
      try
      {
        var resultado = await categoriaService.GetByIdAsync(id);
        return Ok(resultado);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }
  }
}
