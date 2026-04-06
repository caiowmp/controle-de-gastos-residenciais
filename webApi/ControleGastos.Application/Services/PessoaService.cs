using ControleGastos.Application.DTOs.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Interfaces.Services;
using ControleGastos.Domain.Entities;

namespace ControleGastos.Application.Services
{
  /// <summary>
  /// Serviço para gerenciamento de Pessoas.
  /// Implementa as regras de negócio e validações relacionadas ao cadastro de pessoas.
  /// </summary>
  public class PessoaService(
    IPessoaReadOnly pessoaReadOnly,
    IPessoaWriteOnly pessoaWriteOnly,
    IPessoaUpdateOnly pessoaUpdateOnly) : IPessoaService
  {
    /// <summary>
    /// Cria uma nova pessoa no sistema.
    /// Validações:
    /// - Nome é obrigatório (máx. 200 caracteres)
    /// - Idade deve estar entre 0 e 150 anos
    /// </summary>
    /// <param name="createPessoaDto">Dados da pessoa a ser criada</param>
    /// <returns>Dados da pessoa criada</returns>
    /// <exception cref="ArgumentException">Lançado quando os dados são inválidos</exception>
    public async Task<PessoaResponseDto> CreateAsync(CreatePessoaDto createPessoaDto)
    {
      // Validações de negócio
      if (string.IsNullOrWhiteSpace(createPessoaDto.Nome))
        throw new ArgumentException("Nome é obrigatório");

      if (createPessoaDto.Nome.Length > 200)
        throw new ArgumentException("Nome não pode exceder 200 caracteres");

      if (createPessoaDto.Idade < 0 || createPessoaDto.Idade > 150)
        throw new ArgumentException("Idade deve estar entre 0 e 150 anos");

      // Criar entidade
      var pessoa = new Pessoa
      {
        Id = Guid.NewGuid(),
        Nome = createPessoaDto.Nome.Trim(),
        Idade = createPessoaDto.Idade
      };

      // Persistir
      await pessoaWriteOnly.Add(pessoa);

      // Retornar DTO
      return new PessoaResponseDto
      {
        Id = pessoa.Id,
        Nome = pessoa.Nome,
        Idade = pessoa.Idade
      };
    }

    /// <summary>
    /// Obtém uma pessoa específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da pessoa</param>
    /// <returns>Dados da pessoa encontrada</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando a pessoa não é encontrada</exception>
    public async Task<PessoaResponseDto> GetByIdAsync(Guid id)
    {
      var pessoa = await pessoaReadOnly.GetById(id);

      if (pessoa == null)
        throw new KeyNotFoundException($"Pessoa com ID {id} não encontrada");

      return new PessoaResponseDto
      {
        Id = pessoa.Id,
        Nome = pessoa.Nome,
        Idade = pessoa.Idade
      };
    }

    /// <summary>
    /// Obtém todas as pessoas cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as pessoas</returns>
    public async Task<List<PessoaResponseDto>> GetAllAsync()
    {
      var pessoas = await pessoaReadOnly.GetAll();

      return pessoas
        .Select(p => new PessoaResponseDto
        {
          Id = p.Id,
          Nome = p.Nome,
          Idade = p.Idade
        })
        .ToList();
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// Validações:
    /// - Pessoa deve existir
    /// - Nome é obrigatório (máx. 200 caracteres)
    /// - Idade deve estar entre 0 e 150 anos
    /// </summary>
    /// <param name="id">ID da pessoa a ser atualizada</param>
    /// <param name="updatePessoaDto">Novos dados da pessoa</param>
    /// <returns>Dados da pessoa atualizada</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando a pessoa não é encontrada</exception>
    /// <exception cref="ArgumentException">Lançado quando os dados são inválidos</exception>
    public async Task<PessoaResponseDto> UpdateAsync(Guid id, UpdatePessoaDto updatePessoaDto)
    {
      // Verificar se pessoa existe
      var pessoa = await pessoaReadOnly.GetById(id);
      if (pessoa == null)
        throw new KeyNotFoundException($"Pessoa com ID {id} não encontrada");

      // Validações de negócio
      if (string.IsNullOrWhiteSpace(updatePessoaDto.Nome))
        throw new ArgumentException("Nome é obrigatório");

      if (updatePessoaDto.Nome.Length > 200)
        throw new ArgumentException("Nome não pode exceder 200 caracteres");

      if (updatePessoaDto.Idade < 0 || updatePessoaDto.Idade > 150)
        throw new ArgumentException("Idade deve estar entre 0 e 150 anos");

      // Atualizar entidade
      pessoa.Nome = updatePessoaDto.Nome.Trim();
      pessoa.Idade = updatePessoaDto.Idade;

      // Persistir
      await pessoaUpdateOnly.Update(pessoa);

      // Retornar DTO
      return new PessoaResponseDto
      {
        Id = pessoa.Id,
        Nome = pessoa.Nome,
        Idade = pessoa.Idade
      };
    }

    /// <summary>
    /// Deleta uma pessoa e todas as suas transações associadas.
    /// A deleção é em cascata através da configuração do banco de dados.
    /// </summary>
    /// <param name="id">ID da pessoa a ser deletada</param>
    /// <exception cref="KeyNotFoundException">Lançado quando a pessoa não é encontrada</exception>
    public async Task DeleteAsync(Guid id)
    {
      // Verificar se pessoa existe
      var pessoa = await pessoaReadOnly.GetById(id);
      if (pessoa == null)
        throw new KeyNotFoundException($"Pessoa com ID {id} não encontrada");

      // Deletar (cascata remove transações automaticamente)
      await pessoaWriteOnly.Delete(id);
    }
  }
}
