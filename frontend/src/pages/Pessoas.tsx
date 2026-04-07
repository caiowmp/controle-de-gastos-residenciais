import { useState, useEffect } from 'react'
import { getPessoas, createPessoa, updatePessoa, deletePessoa } from '../api/pessoas.ts'
import type { Pessoa } from '../types/Pessoa.ts'

/**
 * Pagina de Gerenciamento de Pessoas
 * 
 * Funcionalidades:
 * - Listar todas as pessoas cadastradas
 * - Criar nova pessoa (Nome + Idade)
 * - Editar dados de uma pessoa existente
 * - Deletar pessoa (remove automaticamente todas as suas transacoes)
 * 
 * Validacoes:
 * - Nome: maximo 200 caracteres
 * - Idade: informada no cadastro
 */
interface FormData {
  nome: string
  idade: string
}

export default function Pessoas() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([])
  const [formData, setFormData] = useState<FormData>({ nome: '', idade: '' })
  const [editingId, setEditingId] = useState<string | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // Carrega a lista de pessoas ao montar o componente
  useEffect(() => {
    loadPessoas()
  }, [])

  const loadPessoas = async () => {
    try {
      setLoading(true)
      setError(null)
      const data = await getPessoas()
      setPessoas(data)
    } catch (err) {
      setError('Erro ao carregar pessoas: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!formData.nome.trim()) {
      setError('Nome eh obrigatorio')
      return
    }

    if (!formData.idade || parseInt(formData.idade) < 0) {
      setError('Idade deve ser um numero positivo')
      return
    }

    if (formData.nome.length > 200) {
      setError('Nome nao pode ter mais de 200 caracteres')
      return
    }

    try {
      setError(null)
      const pessoaData = {
        nome: formData.nome.trim(),
        idade: parseInt(formData.idade)
      }

      if (editingId) {
        await updatePessoa(editingId, pessoaData)
      } else {
        await createPessoa(pessoaData)
      }

      setFormData({ nome: '', idade: '' })
      setEditingId(null)
      await loadPessoas()
    } catch (err) {
      setError('Erro ao salvar pessoa: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    }
  }

  const handleEdit = (pessoa: Pessoa) => {
    setFormData({ nome: pessoa.nome, idade: String(pessoa.idade) })
    setEditingId(pessoa.id)
  }

  const handleDelete = async (id: string) => {
    if (!window.confirm('Tem certeza? Todas as transacoes dessa pessoa serao removidas')) {
      return
    }

    try {
      setError(null)
      await deletePessoa(id)
      await loadPessoas()
    } catch (err) {
      setError('Erro ao deletar pessoa: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    }
  }

  const handleCancel = () => {
    setFormData({ nome: '', idade: '' })
    setEditingId(null)
  }

  return (
    <div className="page">
      <h2>Gerenciamento de Pessoas</h2>

      {/* Formulario de entrada */}
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="nome">Nome:</label>
          <input
            id="nome"
            type="text"
            value={formData.nome}
            onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
            maxLength={200}
            placeholder="Digite o nome da pessoa"
          />
          <small>{formData.nome.length}/200</small>
        </div>

        <div className="form-group">
          <label htmlFor="idade">Idade:</label>
          <input
            id="idade"
            type="number"
            value={formData.idade}
            onChange={(e) => setFormData({ ...formData, idade: e.target.value })}
            min="0"
            max="150"
            placeholder="Digite a idade"
          />
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="form-actions">
          <button type="submit" className="btn btn-primary">
            {editingId ? 'Atualizar' : 'Criar'}
          </button>
          {editingId && (
            <button type="button" onClick={handleCancel} className="btn btn-secondary">
              Cancelar
            </button>
          )}
        </div>
      </form>

      {/* Lista de pessoas */}
      <div className="list-container">
        <h3>Lista de Pessoas ({pessoas.length})</h3>

        {loading && <p className="loading">Carregando...</p>}

        {!loading && pessoas.length === 0 && (
          <p className="empty-message">Nenhuma pessoa cadastrada</p>
        )}

        {!loading && pessoas.length > 0 && (
          <table className="table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nome</th>
                <th>Idade</th>
                <th>Acoes</th>
              </tr>
            </thead>
            <tbody>
              {pessoas.map((pessoa) => (
                <tr key={pessoa.id}>
                  <td>{pessoa.id}</td>
                  <td>{pessoa.nome}</td>
                  <td>{pessoa.idade} anos</td>
                  <td>
                    <button
                      onClick={() => handleEdit(pessoa)}
                      className="btn btn-small btn-secondary"
                    >
                      Editar
                    </button>
                    <button
                      onClick={() => handleDelete(pessoa.id)}
                      className="btn btn-small btn-danger"
                    >
                      Deletar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
