using ControleGastos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.Infrastructure.Data
{
  public class ControleGastosDbContext(DbContextOptions<ControleGastosDbContext> options) : DbContext(options)
  {
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Pessoa>()
          .HasMany(p => p.Transacoes)
          .WithOne()
          .HasForeignKey(t => t.PessoaId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}

