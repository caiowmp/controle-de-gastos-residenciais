import { api } from "./client";
import type { Pessoa } from "../types/Pessoa.ts";

/**
 * Recupera todas as pessoas cadastradas no sistema
 */
export const getPessoas = async (): Promise<Pessoa[]> => {
  const response = await api.get("/pessoa");
  return response.data;
};

/**
 * Cria uma nova pessoa no sistema
 * @param pessoa Dados da pessoa contendo nome (máx 200 chars) e idade
 */
export const createPessoa = async (pessoa: Omit<Pessoa, "id">) => {
  const response = await api.post("/pessoa", pessoa);
  return response.data;
};

/**
 * Atualiza os dados de uma pessoa existente
 * @param id Identificador único da pessoa
 * @param pessoa Novos dados para a pessoa
 */
export const updatePessoa = async (id: string, pessoa: Omit<Pessoa, "id">) => {
  await api.put(`/pessoa/${id}`, pessoa);
};

/**
 * Remove uma pessoa do sistema
 * Nota: Ao deletar uma pessoa, todas as suas transações são removidas automaticamente
 * @param id Identificador único da pessoa a ser removida
 */
export const deletePessoa = async (id: string) => {
  await api.delete(`/pessoa/${id}`);
};