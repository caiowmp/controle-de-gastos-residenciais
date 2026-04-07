import { api } from "./client";
import type { Transacao } from "../types/Transacao.ts";

/**
 * Recupera todas as transações registradas no sistema
 */
export const getTransacoes = async (): Promise<Transacao[]> => {
  const response = await api.get("/transacao");
  return response.data;
};

/**
 * Cria uma nova transação
 * Validações aplicadas no backend:
 * - Se a pessoa é menor de idade, apenas despesas são aceitas
 * - A categoria deve ser compatível com o tipo da transação
 */
export const createTransacao = async (transacao: Omit<Transacao, "id">) => {
  console.log('Criando transacao', transacao)
  const response = await api.post("/transacao", transacao);
  return response.data;
};