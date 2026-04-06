using ControleGastos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Data
{
  /// <summary>
  /// Contexto do Entity Framework Core para a aplicação de Controle de Gastos.
  /// Gerencia os relacionamentos entre as entidades e a persistência dos dados em SQLite.
  /// </summary>
  public class ControleGastosDbContext(DbContextOptions<ControleGastosDbContext> options) : DbContext(options)
  {
    /// <summary>
    /// DbSet para gerenciar as pessoas cadastradas no sistema.
    /// </summary>
    public DbSet<Pessoa> Pessoas { get; set; }

    /// <summary>
    /// DbSet para gerenciar as categorias de transações.
    /// </summary>
    public DbSet<Categoria> Categorias { get; set; }

    /// <summary>
    /// DbSet para gerenciar as transações (receitas e despesas).
    /// </summary>
    public DbSet<Transacao> Transacoes { get; set; }

    /// <summary>
    /// Configura os relacionamentos entre as entidades.
    /// Define comportamento de deleção em cascata e outras propriedades do banco de dados.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Configuração do relacionamento Pessoa → Transacao
      // Quando uma pessoa é deletada, todas suas transações também são deletadas (cascata)
      modelBuilder.Entity<Transacao>()
          .HasOne(t => t.Pessoa)
          .WithMany(p => p.Transacoes)
          .HasForeignKey(t => t.PessoaId)
          .OnDelete(DeleteBehavior.Cascade)
          .IsRequired();

      // Configuração do relacionamento Categoria → Transacao
      // Uma categoria pode ter múltiplas transações
      modelBuilder.Entity<Transacao>()
          .HasOne(t => t.Categoria)
          .WithMany()
          .HasForeignKey(t => t.CategoriaId)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired();

      // Índices para melhorar performance nas buscas
      modelBuilder.Entity<Pessoa>()
          .HasIndex(p => p.Nome)
          .HasDatabaseName("IX_Pessoa_Nome");

      modelBuilder.Entity<Categoria>()
          .HasIndex(c => c.Descricao)
          .HasDatabaseName("IX_Categoria_Descricao");

      modelBuilder.Entity<Transacao>()
          .HasIndex(t => t.PessoaId)
          .HasDatabaseName("IX_Transacao_PessoaId");

      modelBuilder.Entity<Transacao>()
          .HasIndex(t => t.CategoriaId)
          .HasDatabaseName("IX_Transacao_CategoriaId");
    }
  }
}

