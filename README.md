# 📌 Controle de Gastos Residenciais - Backend

Este projeto é uma **Web API desenvolvida em C# com .NET**, responsável por gerenciar o controle de gastos residenciais, permitindo cadastro de pessoas, categorias e transações financeiras, além de fornecer relatórios consolidados.

---

# 🚀 Tecnologias utilizadas

- .NET (ASP.NET Core Web API)
- C#
- Entity Framework Core
- SQL Server (ou outro provider configurado)
- Swagger (documentação da API)
- xUnit (testes unitários)

---

# 🏗 Arquitetura do Projeto

O projeto segue uma arquitetura em camadas, com separação clara de responsabilidades:

ControleGastos

├── API (Controllers)
├── Application (Services / Regras de negócio)
├── Domain (Entidades e enums)
├── Infrastructure (Persistência / EF Core)
├── Tests (Testes unitários)

---

# ⚙️ Requisitos para rodar o projeto

Antes de iniciar, certifique-se de ter instalado:

- .NET SDK (versão 6 ou superior)
- SQL Server (ou outro banco compatível configurado)
- Visual Studio ou VS Code

---

# 🔧 Configuração do ambiente

## 1. Clonar o repositório

```bash
git clone https://github.com/caiowmp/controle-de-gastos-residenciais
cd controle-de-gastos-residenciais/webapi
```

## 2. Aplicar migrations

```bash
dotnet restore
dotnet build
dotnet ef database update --project ControleGastos.Infrastructure --startup-project ControleGastos.Api
```

## 3. Executar o projeto

```bash
dotnet run
```
---

## 4. Acessar o Swagger

https://localhost:5000/swagger

---

# 📌 Funcionalidades implementadas

## 👤 Pessoas
- Criar pessoa
- Listar pessoas
- Editar pessoa
- Deletar pessoa

## 🏷 Categorias
- Criar categoria
- Listar categorias

## 💰 Transações
- Criar transação
- Listar transações

---

# ⚠️ Regras de negócio importantes

- Menores de idade não podem ter receitas
- Categorias devem ser compatíveis com o tipo da transação
- Valor da transação deve ser positivo
- Ao deletar uma pessoa, suas transações são removidas

---

# 📊 Relatórios

## Totais por pessoa
- Total de receitas
- Total de despesas
- Saldo
- Total geral consolidado

---

# 🧪 Testes

Para executar os testes:

```bash
dotnet test
```

---

# 📌 Boas práticas adotadas

- Separação por camadas
- Uso de DTOs
- Injeção de dependência
- Regras de negócio centralizadas nos services

---

# 👨‍💻 Autor

Projeto desenvolvido para avaliação técnica.


# Controle de Gastos Residenciais - Frontend

Frontend desenvolvido em **React + TypeScript + Vite** para o Sistema de Controle de Gastos Residenciais.

## Visão Geral

Aplicação web para gerenciar receitas e despesas de pessoas em uma residência, com funcionalidades de:
- **Cadastro de Pessoas**: Criar, editar, deletar e listar pessoas
- **Cadastro de Categorias**: Criar e listar categorias (Receita, Despesa, Ambas)
- **Cadastro de Transações**: Registrar receitas e despesas com validações de negócio
- **Relatórios**: Consultar totais por pessoa e por categoria

## Tecnologias Utilizadas

- **React 18+** - Biblioteca de UI
- **TypeScript** - Type-safety em JavaScript
- **Vite** - Build tool e dev server
- **Axios** - Cliente HTTP para API calls
- **CSS3** - Estilização com variáveis CSS

## Estrutura do Projeto

```
src/
├── App.tsx                    # Componente principal com navegação
├── App.css                    # Estilos globais
├── main.tsx                   # Ponto de entrada
├── api/                       # Integração com Backend
│   ├── client.ts              # Cliente Axios
│   ├── pessoas.ts             # CRUD de Pessoas
│   ├── categorias.ts          # CRUD de Categorias
│   ├── transacoes.ts          # CRUD de Transações
│   └── relatorios.ts          # Consultas de Relatórios
├── types/                     # Tipos TypeScript
│   ├── Pessoa.ts
│   ├── Categoria.ts
│   └── Transacao.ts
└── pages/                     # Páginas principais
    ├── Pessoas.tsx            # Gerenciamento de Pessoas
    ├── Categorias.tsx         # Gerenciamento de Categorias
    ├── Transacoes.tsx         # Gerenciamento de Transações
    ├── TotaisPorPessoa.tsx    # Relatório por Pessoa
    └── TotaisPorCategoria.tsx # Relatório por Categoria (opcional)
```

## Instalação e Execução

### Pré-requisitos
- Node.js 16+
- npm ou yarn
- Backend rodando em `https://localhost:5000`

### Passos

1. **Instalar dependências**:
   ```bash
   cd frontend
   npm install
   ```

2. **Iniciar servidor de desenvolvimento**:
   ```bash
   npm run dev
   ```

3. **Acessar a aplicação**:
   ```
   http://localhost:5173
   ```

## Funcionalidades Implementadas

### 1. Pessoas
- ✅ Criar pessoa (nome, idade)
- ✅ Editar pessoa
- ✅ Deletar pessoa (remove cascata de transações)
- ✅ Listar pessoas
- ✅ Validação: Nome máximo 200 caracteres

### 2. Categorias
- ✅ Criar categoria (descrição, finalidade)
- ✅ Listar categorias
- ✅ Finalidade: Receita, Despesa, Ambas
- ✅ Validação: Descrição máximo 400 caracteres

### 3. Transações
- ✅ Criar transação (descrição, valor, tipo, categoria, pessoa)
- ✅ Listar transações
- ✅ Filtro de categorias conforme o tipo selecionado
- ✅ **Validação especial**: Menores de 18 anos APENAS despesas
- ✅ **Validação especial**: Compatibilidade Categoria ↔ Tipo
- ✅ Validações: Valor positivo, campos obrigatórios

### 4. Relatórios
- ✅ Totais por Pessoa (receitas, despesas, saldo)
- ✅ Totais por Categoria (receitas, despesas, saldo)
- ✅ Resumo geral de todo o período

## Regras de Negócio Implementadas

### Menores de Idade
```
Quando: Pessoa.idade < 18
Então: Apenas transações do tipo "Despesa" são permitidas
```

### Compatibilidade de Categoria
```
Se Transacao.tipo = "Receita"
  → Categoria.finalidade ≠ "Despesa"

Se Transacao.tipo = "Despesa"
  → Categoria.finalidade ≠ "Receita"
```

### Deleção em Cascata
```
Quando: Uma Pessoa é deletada
Então: Todas as Transacoes dessa Pessoa são removidas automaticamente
```

## Endpoints da API Esperados

```
GET    /api/pessoas
POST   /api/pessoa
PUT    /api/pessoa/{id}
DELETE /api/pessoa/{id}

GET    /api/categoria
POST   /api/categoria

GET    /api/transacoes
POST   /api/transacoes

GET    /api/relatorios/totais-por-pessoa
GET    /api/relatorios/totais-por-categoria
```

## Desenvolvimento

### Scripts Disponíveis

```bash
npm run dev      # Inicia servidor de desenvolvimento
npm run build    # Build para produção
npm run preview  # Preview do build
npm run lint     # Executa ESLint
```

## Notas Importantes

1. A aplicação pressupõe que o Backend está em `https://localhost:5000`
2. Todas as validações críticas de negócio são feitas no Backend
3. O Frontend faz validações de UX e formatação
4. Design responsivo para desktop e mobile

