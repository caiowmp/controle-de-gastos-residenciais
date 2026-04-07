import { api } from "./client";

/**
 * Interface para representar os totais de uma pessoa
 */
export interface TotalPessoa {
  pessoaId: string;
  pessoaNome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

/**
 * Retorna todos os totais por pessoa com resumo geral
 */
export interface RelatorioTotaisPessoa {
  pessoas: TotalPessoa[];
  resumoGeral: {
    totalReceitasGeral: number;
    totalDespesasGeral: number;
    saldoLiquido: number;
  };
}

/**
 * Interface para representar os totais de uma categoria
 */
export interface TotalCategoria {
  categoriaId: string;
  categoriaDescricao: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

/**
 * Retorna todos os totais por categoria com resumo geral
 */
export interface RelatorioTotaisCategoria {
  categorias: TotalCategoria[];
  resumoGeral: {
    totalReceitasGeral: number;
    totalDespesasGeral: number;
    saldoLiquido: number;
  };
}

/**
 * Recupera o relatório de totais por pessoa
 * Mostra receitas, despesas e saldo de cada pessoa
 */
export const getTotaisPorPessoa = async (): Promise<RelatorioTotaisPessoa> => {
  const response = await api.get("/relatorio/totais-por-pessoa");
  return response.data;
};

/**
 * Recupera o relatório de totais por categoria
 * Mostra receitas, despesas e saldo de cada categoria
 */
export const getTotaisPorCategoria = async (): Promise<RelatorioTotaisCategoria> => {
  const response = await api.get("/relatorio/totais-por-categoria");
  return response.data;
};
