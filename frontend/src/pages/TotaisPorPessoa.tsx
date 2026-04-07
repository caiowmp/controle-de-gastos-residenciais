import { useState, useEffect } from 'react'
import { getTotaisPorPessoa, type RelatorioTotaisPessoa } from '../api/relatorios.ts'

/**
 * Pagina de Consulta de Totais por Pessoa
 * 
 * Funcionalidade:
 * - Exibir um relatorio mostrando:
 *   * Total de receitas de cada pessoa
 *   * Total de despesas de cada pessoa
 *   * Saldo (receita - despesa) de cada pessoa
 *   * Resumo geral com totalizacoes de todas as pessoas
 */
export default function TotaisPorPessoa() {
  const [relatorio, setRelatorio] = useState<RelatorioTotaisPessoa | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // Carrega o relatorio ao montar o componente
  useEffect(() => {
    loadRelatorio()
  }, [])

  const loadRelatorio = async () => {
    try {
      setLoading(true)
      setError(null)
      const data = await getTotaisPorPessoa()
      setRelatorio(data)
    } catch (err) {
      setError('Erro ao carregar relatorio: ' + (err instanceof Error ? err.message : 'erro desconhecido'))
    } finally {
      setLoading(false)
    }
  }

  // Formata valor monetario
  const formatarMoeda = (valor: number): string => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(valor)
  }

  // Retorna a classe CSS para colorir o saldo
  const getClasseSaldo = (saldo: number): string => {
    if (saldo > 0) return 'text-green'
    if (saldo < 0) return 'text-red'
    return 'text-neutral'
  }

  return (
    <div className="page">
      <h2>Relatório de Totais por Pessoa</h2>

      {loading && <p className="loading">Carregando relatório...</p>}

      {error && <div className="error-message">{error}</div>}

      {!loading && relatorio && (
        <>
          {/* Tabela de pessoas */}
          <div className="list-container">
            <h3>Detalhamento por Pessoa ({relatorio.pessoas.length})</h3>

            {relatorio.pessoas.length === 0 ? (
              <p className="empty-message">Nenhuma pessoa cadastrada</p>
            ) : (
              <table className="table">
                <thead>
                  <tr>
                    <th>Pessoa</th>
                    <th>Total de Receitas</th>
                    <th>Total de Despesas</th>
                    <th>Saldo</th>
                  </tr>
                </thead>
                <tbody>
                  {relatorio.pessoas.map((pessoa) => (
                    <tr key={pessoa.pessoaId}>
                      <td>{pessoa.nomePessoa}</td>
                      <td className="text-green">{formatarMoeda(pessoa.totalReceitas)}</td>
                      <td className="text-red">{formatarMoeda(pessoa.totalDespesas)}</td>
                      <td className={getClasseSaldo(pessoa.saldo)}>
                        <strong>{formatarMoeda(pessoa.saldo)}</strong>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>

          {/* Resumo Geral */}
          {relatorio.totaisGerais ? (
            <div className="summary-container">
              <h3>Resumo Geral</h3>
              <div className="summary-grid">
                <div className="summary-card">
                  <h4>Total de Receitas</h4>
                  <p className="text-green summary-value">
                    {formatarMoeda(relatorio.totaisGerais.totalReceitas)}
                  </p>
                </div>
                <div className="summary-card">
                  <h4>Total de Despesas</h4>
                  <p className="text-red summary-value">
                    {formatarMoeda(relatorio.totaisGerais.totalDespesas)}
                  </p>
                </div>
                <div className={`summary-card ${getClasseSaldo(relatorio.totaisGerais.saldo)}`}>
                  <h4>Saldo Liquido</h4>
                  <p className={`${getClasseSaldo(relatorio.totaisGerais.saldo)} summary-value`}>
                    {formatarMoeda(relatorio.totaisGerais.saldo)}
                  </p>
                </div>
              </div>
            </div>
          ) : (
            <p className="error-message">Resumo geral não disponível</p>
          )}

          {/* Botao para atualizar */}
          <div className="form-actions">
            <button onClick={loadRelatorio} className="btn btn-primary">
              Atualizar Relatorio
            </button>
          </div>
        </>
      )}
    </div>
  )
}
