import { useState, useEffect } from 'react'
import { getTransacoes, createTransacao } from '../api/transacoes.ts'
import { getPessoas } from '../api/pessoas.ts'
import { getCategorias } from '../api/categorias.ts'
import { TipoTransacao } from '../types/Transacao.ts'
import type { Transacao } from '../types/Transacao.ts'
import type { Pessoa } from '../types/Pessoa.ts'
import { Finalidade } from '../types/Categoria.ts'
import type { Categoria } from '../types/Categoria.ts'

/**
 * Pagina de Gerenciamento de Transacoes
 * 
 * Funcionalidades:
 * - Listar todas as transacoes cadastradas
 * - Criar nova transacao com validacoes de negocio
 * 
 * Validacoes aplicadas:
 * - Se a pessoa eh menor de idade (< 18 anos), apenas despesas sao permitidas
 * - A categoria selecionada deve ser compativel com o tipo de transacao
 * - Valor deve ser positivo
 * - Descricao eh obrigatoria
 * 
 * Regras de compatibilidade de categoria:
 * - Se transacao eh "Receita", categoria nao pode ter finalidade "Despesa"
 * - Se transacao eh "Despesa", categoria nao pode ter finalidade "Receita"
 */
interface FormData {
  descricao: string
  valor: string
  tipo: TipoTransacao
  categoriaId: string
  pessoaId: string
}

export default function Transacoes() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([])
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [formData, setFormData] = useState<FormData>({
    descricao: '',
    valor: '',
    tipo: TipoTransacao.Despesa,
    categoriaId: '',
    pessoaId: ''
  })
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // Carrega dados ao montar o componente
  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      setLoading(true)
      setError(null)
      const [transacoesData, pessoasData, categoriasData] = await Promise.all([
        getTransacoes(),
        getPessoas(),
        getCategorias()
      ])
      setTransacoes(transacoesData)
      setPessoas(pessoasData)
      setCategorias(categoriasData)
    } catch (err) {
      setError('Erro ao carregar dados: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    } finally {
      setLoading(false)
    }
  }

  // Valida se a categoria eh compativel com o tipo de transacao
  const isCategoriaCompativel = (categoriaId: string, tipo: TipoTransacao): boolean => {
    const categoria = categorias.find(c => c.id === categoriaId)

    if (!categoria) {
      console.warn(`Categoria ${categoriaId} não encontrada`)
      return false
    }

    // Converter para strings para comparação segura
    const finalidadeStr = String(categoria.finalidade)
    const tipoStr = String(tipo)

    // Se a categoria é "Ambas", sempre é compatível
    if (finalidadeStr === Finalidade.Ambas) {
      return true
    }

    // Se a categoria é "Receita", só pode ser usada em Receita
    if (finalidadeStr === Finalidade.Receita && tipoStr !== TipoTransacao.Receita) {
      return false
    }

    // Se a categoria é "Despesa", só pode ser usada em Despesa
    if (finalidadeStr === Finalidade.Despesa && tipoStr !== TipoTransacao.Despesa) {
      return false
    }

    return true
  }

  // Valida se a pessoa pode realizar transacoes do tipo selecionado
  const isPessoaPermitida = (pessoaId: string, tipo: TipoTransacao): boolean => {
    const pessoa = pessoas.find(p => p.id === pessoaId)
    if (!pessoa) return true // Se não encontrar, permite (não deve bloquear)

    // Menores de idade nao podem fazer receitas
    const idade = typeof pessoa.idade === 'string' ? parseInt(pessoa.idade) : pessoa.idade
    const tipoStr = String(tipo)
    
    if (idade < 18 && tipoStr === TipoTransacao.Receita) {
      return false
    }

    return true
  }

  // Retorna as categorias que podem ser usadas com o tipo selecionado
  const getCategoriasFiltradas = (): Categoria[] => {
    const tipo = formData.tipo
    const tipoStr = String(tipo)

    return categorias.filter(cat => {
      const finalidadeStr = String(cat.finalidade)

      // Se é "Ambas", sempre aparece
      if (finalidadeStr === Finalidade.Ambas) {
        return true
      }

      // Se é "Receita", só aparece em transações de Receita
      if (finalidadeStr === Finalidade.Receita) {
        return tipoStr === TipoTransacao.Receita
      }

      // Se é "Despesa", só aparece em transações de Despesa
      if (finalidadeStr === Finalidade.Despesa) {
        return tipoStr === TipoTransacao.Despesa
      }

      return false
    })
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    // Validacao: pessoa obrigatoria
    if (!formData.pessoaId) {
      setError('Pessoa eh obrigatoria')
      return
    }

    // Validacao: descricao obrigatoria
    if (!formData.descricao.trim()) {
      setError('Descricao eh obrigatoria')
      return
    }

    // Validacao: descricao maxima
    if (formData.descricao.length > 400) {
      setError('Descricao nao pode ter mais de 400 caracteres')
      return
    }

    // Validacao: valor obrigatorio e positivo
    if (!formData.valor || parseFloat(formData.valor) <= 0) {
      setError('Valor deve ser um numero positivo')
      return
    }

    // Validacao: categoria obrigatoria
    if (!formData.categoriaId) {
      setError('Categoria é obrigatoria')
      return
    }

    // Validacao: pessoa menor de idade nao pode fazer receita
    if (formData.tipo === TipoTransacao.Receita && !isPessoaPermitida(formData.pessoaId, formData.tipo)) {
      setError('Pessoas menores de 18 anos não podem fazer receitas')
      return
    }

    // Validacao: categoria compativel com o tipo
    if (!isCategoriaCompativel(formData.categoriaId, formData.tipo)) {
      setError('A categoria selecionada não é compatível com o tipo de transação')
      return
    }

    try {
      setError(null)
      const transacaoData = {
        descricao: formData.descricao.trim(),
        valor: parseFloat(formData.valor),
        tipo: formData.tipo,
        categoriaId: formData.categoriaId,
        pessoaId: formData.pessoaId
      }

      await createTransacao(transacaoData)
      setFormData({
        descricao: '',
        valor: '',
        tipo: TipoTransacao.Despesa,
        categoriaId: '',
        pessoaId: ''
      })
      await loadData()
    } catch (err) {
      setError('Erro ao criar transacao: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    }
  }

  return (
    <div className="page">
      <h2>Gerenciamento de Transacoes</h2>

      {/* Formulario de entrada */}
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="pessoa">Pessoa:</label>
          <select
            id="pessoa"
            value={formData.pessoaId}
            onChange={(e) => setFormData({ ...formData, pessoaId: e.target.value })}
          >
            <option value="">Selecione uma pessoa</option>
            {pessoas.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nome} ({p.idade} anos) {p.idade < 18 ? '- MENOR DE IDADE' : ''}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="tipo">Tipo:</label>
          <select
            id="tipo"
            value={formData.tipo}
            onChange={(e) => {
              setFormData({ ...formData, tipo: e.target.value as TipoTransacao, categoriaId: '' })
              setError(null)
            }}
          >
            <option value={TipoTransacao.Despesa}>Despesa</option>
            <option value={TipoTransacao.Receita}>Receita</option>
          </select>
        </div>

        {formData.pessoaId && formData.tipo === TipoTransacao.Receita && !isPessoaPermitida(formData.pessoaId, formData.tipo) && (
          <div className="warning-message">
            Atenção: Esta pessoa é menor de 18 anos e nao pode fazer receitas
          </div>
        )}

        <div className="form-group">
          <label htmlFor="categoria">Categoria:</label>
          <select
            id="categoria"
            value={formData.categoriaId}
            onChange={(e) => setFormData({ ...formData, categoriaId: e.target.value })}
          >
            <option value="">Selecione uma categoria</option>
            {getCategoriasFiltradas().map((c) => (
              <option key={c.id} value={c.id}>
                {c.descricao}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="descricao">Descrição:</label>
          <input
            id="descricao"
            type="text"
            value={formData.descricao}
            onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
            maxLength={400}
            placeholder="Digite a descrição da transação"
          />
          <small>{formData.descricao.length}/400</small>
        </div>

        <div className="form-group">
          <label htmlFor="valor">Valor (R$):</label>
          <input
            id="valor"
            type="number"
            step="0.01"
            value={formData.valor}
            onChange={(e) => setFormData({ ...formData, valor: e.target.value })}
            min="0"
            placeholder="0.00"
          />
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="form-actions">
          <button type="submit" className="btn btn-primary">
            Criar Transação
          </button>
        </div>
      </form>

      {/* Lista de transacoes */}
      <div className="list-container">
        <h3>Lista de Transações ({transacoes.length})</h3>

        {loading && <p className="loading">Carregando...</p>}

        {!loading && transacoes.length === 0 && (
          <p className="empty-message">Nenhuma transação cadastrada</p>
        )}

        {!loading && transacoes.length > 0 && (
          <table className="table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Pessoa</th>
                <th>Descricao</th>
                <th>Tipo</th>
                <th>Categoria</th>
                <th>Valor</th>
              </tr>
            </thead>
            <tbody>
              {transacoes.map((transacao) => {                
                return (
                  <tr key={transacao.id}>
                    <td>{transacao.id}</td>
                    <td>{transacao.nomePessoa}</td>
                    <td>{transacao.descricao}</td>
                    <td>
                      <span className={`badge badge-${transacao.tipo.toLowerCase()}`}>
                        {transacao.tipo}
                      </span>
                    </td>
                    <td>{transacao.categoriaDescricao}</td>
                    <td className={transacao.tipo === TipoTransacao.Receita ? 'text-green' : 'text-red'}>
                      {transacao.tipo === TipoTransacao.Receita ? '+' : '-'} R$ {transacao.valor.toFixed(2)}
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
