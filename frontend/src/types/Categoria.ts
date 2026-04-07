export const Finalidade = {
  Receita: "Receita",
  Despesa: "Despesa",
  Ambas: "Ambas"
} as const;

export type Finalidade = typeof Finalidade[keyof typeof Finalidade];

export interface Categoria {
  id: string;
  descricao: string;
  finalidade: Finalidade;
}