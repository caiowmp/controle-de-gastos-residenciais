using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ControleGastos.Domain.Enums;

namespace ControleGastos.Domain.Entities
{
  /// <summary>
  /// Entidade que representa uma transação financeira (receita ou despesa).
  /// Uma transação está sempre associada a uma pessoa e uma categoria.
  /// </summary>
  public class Transacao
  {
    /// <summary>
    /// Identificador único da transação. Gerado automaticamente como GUID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição da transação. Campo obrigatório com máximo de 400 caracteres.
    /// Ex: "Compra de supermercado", "Salário de junho", etc.
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(400, MinimumLength = 1, ErrorMessage = "Descrição deve ter entre 1 e 400 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor da transação. Deve ser um valor positivo.
    /// Campo obrigatório.
    /// </summary>
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "Valor deve ser maior que zero")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Valor { get; set; }

    /// <summary>
    /// Tipo da transação: Receita ou Despesa.
    /// Menores de idade (< 18 anos) só podem ter despesas.
    /// </summary>
    [Required(ErrorMessage = "Tipo é obrigatório")]
    [EnumDataType(typeof(TipoTransacao), ErrorMessage = "Tipo deve ser Receita ou Despesa")]
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// Identificador da pessoa associada a esta transação.
    /// Chave estrangeira para a tabela Pessoas.
    /// </summary>
    [Required(ErrorMessage = "PessoaId é obrigatório")]
    public Guid PessoaId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a entidade Pessoa.
    /// Permite acessar os dados da pessoa associada à transação.
    /// </summary>
    [ForeignKey(nameof(PessoaId))]
    public Pessoa? Pessoa { get; set; }

    /// <summary>
    /// Identificador da categoria associada a esta transação.
    /// Chave estrangeira para a tabela Categorias.
    /// </summary>
    [Required(ErrorMessage = "CategoriaId é obrigatório")]
    public Guid CategoriaId { get; set; }

    /// <summary>
    /// Propriedade de navegação para a entidade Categoria.
    /// Permite acessar os dados da categoria associada à transação.
    /// </summary>
    [ForeignKey(nameof(CategoriaId))]
    public Categoria? Categoria { get; set; }
  }
}
