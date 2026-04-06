using System.ComponentModel.DataAnnotations;

namespace ControleGastos.Domain.Entities
{
  /// <summary>
  /// Entidade que representa uma pessoa no sistema de controle de gastos.
  /// Uma pessoa pode ter múltiplas transações associadas.
  /// </summary>
  public class Pessoa
  {
    /// <summary>
    /// Identificador único da pessoa. Gerado automaticamente como GUID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome da pessoa. Campo obrigatório com máximo de 200 caracteres.
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Nome deve ter entre 1 e 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa. Campo obrigatório.
    /// </summary>
    [Required(ErrorMessage = "Idade é obrigatória")]
    [Range(0, 150, ErrorMessage = "Idade deve estar entre 0 e 150 anos")]
    public int Idade { get; set; }

    /// <summary>
    /// Coleção de transações associadas a esta pessoa.
    /// Quando a pessoa é deletada, todas suas transações são removidas em cascata.
    /// </summary>
    public List<Transacao> Transacoes { get; set; } = new();
  }
}
