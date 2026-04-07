export const TipoTransacao = {
  Receita: "Receita",
  Despesa: "Despesa",
} as const;

export type TipoTransacao = typeof TipoTransacao[keyof typeof TipoTransacao];


export interface Transacao {
  id: string;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  categoriaId: string;
  pessoaId: string;
}