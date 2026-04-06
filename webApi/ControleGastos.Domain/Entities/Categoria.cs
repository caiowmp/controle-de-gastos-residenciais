using System.ComponentModel.DataAnnotations;
using ControleGastos.Domain.Enums;

namespace ControleGastos.Domain.Entities
{
  /// <summary>
  /// Entidade que representa uma categoria de transação.
  /// As categorias podem ser para Receita, Despesa ou Ambas.
  /// </summary>
  public class Categoria
  {
    /// <summary>
    /// Identificador único da categoria. Gerado automaticamente como GUID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da categoria. Campo obrigatório com máximo de 400 caracteres.
    /// Ex: "Salário", "Alimentação", "Transporte", etc.
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(400, MinimumLength = 1, ErrorMessage = "Descrição deve ter entre 1 e 400 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Define a finalidade da categoria: Receita, Despesa ou Ambas.
    /// Isto é usado para validar quais categorias podem ser usadas em cada tipo de transação.
    /// </summary>
    [Required(ErrorMessage = "Finalidade é obrigatória")]
    [EnumDataType(typeof(FinalidadeCategoria), ErrorMessage = "Finalidade deve ser Receita, Despesa ou Ambas")]
    public FinalidadeCategoria Finalidade { get; set; }
  }
}
