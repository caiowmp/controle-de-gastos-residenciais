import { useState } from "react"
import "./App.css"
import Pessoas from "./pages/Pessoas"
import Categorias from "./pages/Categorias"
import Transacoes from "./pages/Transacoes"
import TotaisPorPessoa from "./pages/TotaisPorPessoa"
import TotaisPorCategoria from "./pages/TotaisPorCategoria"

/**
 * Componente principal da aplicacao de Controle de Gastos Residenciais
 * 
 * Responsabilidades:
 * - Gerenciar a navegacao entre paginas
 * - Renderizar a pagina ativa de acordo com a selecao do usuario
 * - Fornecer acesso a todas as funcionalidades do sistema
 */
type Page = "pessoas" | "categorias" | "transacoes" | "totaisPessoa" | "totaisCategoria"

function App() {
  const [currentPage, setCurrentPage] = useState<Page>("pessoas")

  // Renderiza a pagina de acordo com a selecao
  const renderPage = () => {
    switch (currentPage) {
      case "pessoas":
        return <Pessoas />
      case "categorias":
        return <Categorias />
      case "transacoes":
        return <Transacoes />
      case "totaisPessoa":
        return <TotaisPorPessoa />
      case "totaisCategoria":
        return <TotaisPorCategoria />
      default:
        return <Pessoas />
    }
  }

  return (
    <div className="app-container">
      <nav className="navbar">
        <div className="nav-brand">
          <h1>Sistema de Controle de Gastos</h1>
        </div>
        <ul className="nav-menu">
          <li>
            <button
              className={`nav-button ${currentPage === "pessoas" ? "active" : ""}`}
              onClick={() => setCurrentPage("pessoas")}
            >
              Pessoas
            </button>
          </li>
          <li>
            <button
              className={`nav-button ${currentPage === "categorias" ? "active" : ""}`}
              onClick={() => setCurrentPage("categorias")}
            >
              Categorias
            </button>
          </li>
          <li>
            <button
              className={`nav-button ${currentPage === "transacoes" ? "active" : ""}`}
              onClick={() => setCurrentPage("transacoes")}
            >
              Transacoes
            </button>
          </li>
          <li>
            <button
              className={`nav-button ${currentPage === "totaisPessoa" ? "active" : ""}`}
              onClick={() => setCurrentPage("totaisPessoa")}
            >
              Totais por Pessoa
            </button>
          </li>
          <li>
            <button
              className={`nav-button ${currentPage === "totaisCategoria" ? "active" : ""}`}
              onClick={() => setCurrentPage("totaisCategoria")}
            >
              Totais por Categoria
            </button>
          </li>
        </ul>
      </nav>

      <main className="page-content">
        {renderPage()}
      </main>
    </div>
  )
}

export default App
