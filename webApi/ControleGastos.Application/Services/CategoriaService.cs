using ControleGastos.Application.DTOs.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Interfaces.Services;
using ControleGastos.Domain.Entities;
using ControleGastos.Domain.Enums;

namespace ControleGastos.Application.Services
{
  /// <summary>
  /// Serviço para gerenciamento de Categorias.
  /// Implementa as regras de negócio e validações relacionadas ao cadastro de categorias.
  /// </summary>
  public class CategoriaService(
    ICategoriaReadOnly categoriaReadOnly,
    ICategoriaWriteOnly categoriaWriteOnly) : ICategoriaService
  {
    /// <summary>
    /// Cria uma nova categoria no sistema.
    /// Validações:
    /// - Descrição é obrigatória (máx. 400 caracteres)
    /// - Finalidade deve ser Receita, Despesa ou Ambas
    /// </summary>
    /// <param name="createCategoriaDto">Dados da categoria a ser criada</param>
    /// <returns>Dados da categoria criada</returns>
    /// <exception cref="ArgumentException">Lançado quando os dados são inválidos</exception>
    public async Task<CategoriaResponseDto> CreateAsync(CreateCategoriaDto createCategoriaDto)
    {
      // Validações de negócio
      if (string.IsNullOrWhiteSpace(createCategoriaDto.Descricao))
        throw new ArgumentException("Descrição é obrigatória");

      if (createCategoriaDto.Descricao.Length > 400)
        throw new ArgumentException("Descrição não pode exceder 400 caracteres");

      // Validar enum
      if (!Enum.IsDefined(typeof(FinalidadeCategoria), createCategoriaDto.Finalidade))
        throw new ArgumentException("Finalidade deve ser Receita, Despesa ou Ambas");

      // Criar entidade
      var categoria = new Categoria
      {
        Id = Guid.NewGuid(),
        Descricao = createCategoriaDto.Descricao.Trim(),
        Finalidade = createCategoriaDto.Finalidade
      };

      // Persistir
      await categoriaWriteOnly.Add(categoria);

      // Retornar DTO
      return new CategoriaResponseDto
      {
        Id = categoria.Id,
        Descricao = categoria.Descricao,
        Finalidade = categoria.Finalidade
      };
    }

    /// <summary>
    /// Obtém todas as categorias cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as categorias</returns>
    public async Task<List<CategoriaResponseDto>> GetAllAsync()
    {
      var categorias = await categoriaReadOnly.GetAll();

      return categorias
        .Select(c => new CategoriaResponseDto
        {
          Id = c.Id,
          Descricao = c.Descricao,
          Finalidade = c.Finalidade
        })
        .ToList();
    }

    /// <summary>
    /// Obtém uma categoria específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da categoria</param>
    /// <returns>Dados da categoria encontrada</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando a categoria não é encontrada</exception>
    public async Task<CategoriaResponseDto> GetByIdAsync(Guid id)
    {
      var categoria = await categoriaReadOnly.GetById(id);

      if (categoria == null)
        throw new KeyNotFoundException($"Categoria com ID {id} não encontrada");

      return new CategoriaResponseDto
      {
        Id = categoria.Id,
        Descricao = categoria.Descricao,
        Finalidade = categoria.Finalidade
      };
    }
  }
}
