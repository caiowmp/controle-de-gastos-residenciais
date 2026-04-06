using ControleGastos.Application.DTOs.Transacao;
using ControleGastos.Application.Interfaces.Repositories.Categoria;
using ControleGastos.Application.Interfaces.Repositories.Pessoa;
using ControleGastos.Application.Interfaces.Repositories.Transacao;
using ControleGastos.Application.Interfaces.Services;
using ControleGastos.Domain.Entities;
using ControleGastos.Domain.Enums;

namespace ControleGastos.Application.Services
{
  /// <summary>
  /// Serviço para gerenciamento de Transações.
  /// Implementa as regras de negócio críticas do sistema de controle de gastos.
  /// Valida restrições como: menores de idade não podem ter receitas, 
  /// e categorias devem ser compatíveis com o tipo de transação.
  /// </summary>
  public class TransacaoService(
    ITransacaoReadOnly transacaoReadOnly,
    ITransacaoWriteOnly transacaoWriteOnly,
    IPessoaReadOnly pessoaReadOnly,
    ICategoriaReadOnly categoriaReadOnly) : ITransacaoService
  {
    /// <summary>
    /// Cria uma nova transação no sistema com validações rigorosas.
    /// 
    /// Validações de negócio CRÍTICAS:
    /// 1. RESTRIÇÃO DE MENORES: Menores de 18 anos SÓ podem ter despesas
    /// 2. COMPATIBILIDADE DE CATEGORIA: A categoria deve ser compatível com o tipo
    ///    - Despesa: Categoria deve ter Finalidade = Despesa ou Ambas
    ///    - Receita: Categoria deve ter Finalidade = Receita ou Ambas
    /// 3. DADOS VÁLIDOS: Descrição (máx 400 chars), Valor (positivo)
    /// 4. REFERÊNCIAS EXISTEM: Pessoa e Categoria devem existir
    /// </summary>
    /// <param name="createTransacaoDto">Dados da transação a ser criada</param>
    /// <returns>Dados da transação criada</returns>
    /// <exception cref="ArgumentException">Lançado quando uma validação de negócio falha</exception>
    /// <exception cref="KeyNotFoundException">Lançado quando pessoa ou categoria não existem</exception>
    public async Task<TransacaoResponseDto> CreateAsync(CreateTransacaoDto createTransacaoDto)
    {
      // Validação 1: Pessoa existe?
      var pessoa = await pessoaReadOnly.GetById(createTransacaoDto.PessoaId);
      if (pessoa == null)
        throw new KeyNotFoundException($"Pessoa com ID {createTransacaoDto.PessoaId} não encontrada");

      // Validação 2: Categoria existe?
      var categoria = await categoriaReadOnly.GetById(createTransacaoDto.CategoriaId);
      if (categoria == null)
        throw new KeyNotFoundException($"Categoria com ID {createTransacaoDto.CategoriaId} não encontrada");

      // VALIDAÇÃO CRÍTICA: Menor de 18 anos não pode ter RECEITA
      if (pessoa.Idade < 18 && createTransacaoDto.Tipo == TipoTransacao.Receita)
        throw new ArgumentException(
          $"Menores de 18 anos ({pessoa.Nome} tem {pessoa.Idade} anos) não podem ter transações de receita. " +
          "Apenas despesas são permitidas.");

      // VALIDAÇÃO CRÍTICA: Compatibilidade de categoria com tipo de transação
      if (createTransacaoDto.Tipo == TipoTransacao.Despesa)
      {
        // Para despesa, categoria deve ser Despesa ou Ambas
        if (categoria.Finalidade != FinalidadeCategoria.Despesa && 
            categoria.Finalidade != FinalidadeCategoria.Ambas)
          throw new ArgumentException(
            $"A categoria '{categoria.Descricao}' não é permitida para despesas. " +
            "Use uma categoria com finalidade 'Despesa' ou 'Ambas'.");
      }
      else if (createTransacaoDto.Tipo == TipoTransacao.Receita)
      {
        // Para receita, categoria deve ser Receita ou Ambas
        if (categoria.Finalidade != FinalidadeCategoria.Receita && 
            categoria.Finalidade != FinalidadeCategoria.Ambas)
          throw new ArgumentException(
            $"A categoria '{categoria.Descricao}' não é permitida para receitas. " +
            "Use uma categoria com finalidade 'Receita' ou 'Ambas'.");
      }

      // Validações gerais
      if (string.IsNullOrWhiteSpace(createTransacaoDto.Descricao))
        throw new ArgumentException("Descrição é obrigatória");

      if (createTransacaoDto.Descricao.Length > 400)
        throw new ArgumentException("Descrição não pode exceder 400 caracteres");

      if (createTransacaoDto.Valor <= 0)
        throw new ArgumentException("Valor deve ser maior que zero");

      // Criar entidade
      var transacao = new Transacao
      {
        Id = Guid.NewGuid(),
        Descricao = createTransacaoDto.Descricao.Trim(),
        Valor = createTransacaoDto.Valor,
        Tipo = createTransacaoDto.Tipo,
        PessoaId = createTransacaoDto.PessoaId,
        CategoriaId = createTransacaoDto.CategoriaId
      };

      // Persistir
      await transacaoWriteOnly.Add(transacao);

      // Retornar DTO
      return new TransacaoResponseDto
      {
        Id = transacao.Id,
        Descricao = transacao.Descricao,
        Valor = transacao.Valor,
        Tipo = transacao.Tipo,
        NomePessoa = pessoa.Nome,
        CategoriaDescricao = categoria.Descricao
      };
    }

    /// <summary>
    /// Obtém todas as transações cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de todas as transações com dados de pessoa e categoria</returns>
    public async Task<List<TransacaoResponseDto>> GetAllAsync()
    {
      var transacoes = await transacaoReadOnly.GetAll();

      return transacoes
        .Select(t => new TransacaoResponseDto
        {
          Id = t.Id,
          Descricao = t.Descricao,
          Valor = t.Valor,
          Tipo = t.Tipo,
          NomePessoa = t.Pessoa?.Nome ?? "Pessoa não encontrada",
          CategoriaDescricao = t.Categoria?.Descricao ?? "Categoria não encontrada"
        })
        .ToList();
    }

    /// <summary>
    /// Obtém uma transação específica pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da transação</param>
    /// <returns>Dados da transação encontrada</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando a transação não é encontrada</exception>
    public async Task<TransacaoResponseDto> GetByIdAsync(Guid id)
    {
      var transacao = await transacaoReadOnly.GetById(id);

      if (transacao == null)
        throw new KeyNotFoundException($"Transação com ID {id} não encontrada");

      return new TransacaoResponseDto
      {
        Id = transacao.Id,
        Descricao = transacao.Descricao,
        Valor = transacao.Valor,
        Tipo = transacao.Tipo,
        NomePessoa = transacao.Pessoa?.Nome ?? "Pessoa não encontrada",
        CategoriaDescricao = transacao.Categoria?.Descricao ?? "Categoria não encontrada"
      };
    }

    /// <summary>
    /// Obtém todas as transações de uma pessoa específica.
    /// </summary>
    /// <param name="pessoaId">ID da pessoa</param>
    /// <returns>Lista de transações da pessoa</returns>
    /// <exception cref="KeyNotFoundException">Lançado quando a pessoa não é encontrada</exception>
    public async Task<List<TransacaoResponseDto>> GetByPessoaIdAsync(Guid pessoaId)
    {
      // Validar que pessoa existe
      var pessoa = await pessoaReadOnly.GetById(pessoaId);
      if (pessoa == null)
        throw new KeyNotFoundException($"Pessoa com ID {pessoaId} não encontrada");

      var transacoes = await transacaoReadOnly.GetByPessoaId(pessoaId);

      return transacoes
        .Select(t => new TransacaoResponseDto
        {
          Id = t.Id,
          Descricao = t.Descricao,
          Valor = t.Valor,
          Tipo = t.Tipo,
          NomePessoa = pessoa.Nome,
          CategoriaDescricao = t.Categoria?.Descricao ?? "Categoria não encontrada"
        })
        .ToList();
    }
  }
}
