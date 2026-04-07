import { useState, useEffect } from 'react'
import { getCategorias, createCategoria } from '../api/categorias.ts'
import type { Categoria } from '../types/Categoria.ts'
import { Finalidade } from '../types/Categoria.ts'

/**
 * Pagina de Gerenciamento de Categorias
 * 
 * Funcionalidades:
 * - Listar todas as categorias cadastradas
 * - Criar nova categoria (Descricao + Finalidade)
 * 
 * A finalidade determina se a categoria pode ser usada em:
 * - Receita: apenas para transacoes de receita
 * - Despesa: apenas para transacoes de despesa
 * - Ambas: pode ser usada tanto em receitas quanto despesas
 * 
 * Validacoes:
 * - Descricao: maximo 400 caracteres
 */
interface FormData {
  descricao: string
  finalidade: Finalidade
}

export default function Categorias() {
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [formData, setFormData] = useState<FormData>({
    descricao: '',
    finalidade: Finalidade.Ambas
  })
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // Carrega a lista de categorias ao montar o componente
  useEffect(() => {
    loadCategorias()
  }, [])

  const loadCategorias = async () => {
    try {
      setLoading(true)
      setError(null)
      const data = await getCategorias()
      setCategorias(data)
    } catch (err) {
      setError('Erro ao carregar categorias: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!formData.descricao.trim()) {
      setError('Descricao eh obrigatoria')
      return
    }

    if (formData.descricao.length > 400) {
      setError('Descricao nao pode ter mais de 400 caracteres')
      return
    }

    try {
      setError(null)
      const categoriaData = {
        descricao: formData.descricao.trim(),
        finalidade: formData.finalidade
      }

      await createCategoria(categoriaData)
      setFormData({ descricao: '', finalidade: Finalidade.Ambas })
      await loadCategorias()
    } catch (err) {
      setError('Erro ao criar categoria: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    }
  }

  return (
    <div className="page">
      <h2>Gerenciamento de Categorias</h2>

      {/* Formulario de entrada */}
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label htmlFor="descricao">Descricao:</label>
          <input
            id="descricao"
            type="text"
            value={formData.descricao}
            onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
            maxLength={400}
            placeholder="Digite a descricao da categoria"
          />
          <small>{formData.descricao.length}/400</small>
        </div>

        <div className="form-group">
          <label htmlFor="finalidade">Finalidade:</label>
          <select
            id="finalidade"
            value={formData.finalidade}
            onChange={(e) => setFormData({ ...formData, finalidade: e.target.value as Finalidade })}
          >
            <option value={Finalidade.Receita}>Receita</option>
            <option value={Finalidade.Despesa}>Despesa</option>
            <option value={Finalidade.Ambas}>Ambas</option>
          </select>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="form-actions">
          <button type="submit" className="btn btn-primary">
            Criar Categoria
          </button>
        </div>
      </form>

      {/* Lista de categorias */}
      <div className="list-container">
        <h3>Lista de Categorias ({categorias.length})</h3>

        {loading && <p className="loading">Carregando...</p>}

        {!loading && categorias.length === 0 && (
          <p className="empty-message">Nenhuma categoria cadastrada</p>
        )}

        {!loading && categorias.length > 0 && (
          <table className="table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Descricao</th>
                <th>Finalidade</th>
              </tr>
            </thead>
            <tbody>
              {categorias.map((categoria) => (
                <tr key={categoria.id}>
                  <td>{categoria.id}</td>
                  <td>{categoria.descricao}</td>
                  <td>
                    <span className={`badge badge-${categoria.finalidade.toLowerCase()}`}>
                      {categoria.finalidade}
                    </span>
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
