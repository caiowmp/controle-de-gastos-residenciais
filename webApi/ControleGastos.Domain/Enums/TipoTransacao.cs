using System.Text.Json.Serialization;

namespace ControleGastos.Domain.Enums
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum TipoTransacao
  {
    Receita,
    Despesa
  }
}
