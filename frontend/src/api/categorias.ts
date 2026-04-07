import { api } from "./client";
import type { Categoria } from "../types/Categoria.ts";

/**
 * Recupera todas as categorias cadastradas no sistema
 */
export const getCategorias = async (): Promise<Categoria[]> => {
  const response = await api.get("/categoria");
  return response.data;
};

/**
 * Cria uma nova categoria no sistema
 * @param categoria Dados da categoria contendo:
 *  - descricao: texto até 400 caracteres
 *  - finalidade: "Receita" | "Despesa" | "Ambas"
 */
export const createCategoria = async (categoria: Omit<Categoria, "id">) => {
  const response = await api.post("/categoria", categoria);
  return response.data;
};