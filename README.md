# StockHive API 📦

![.NET](https://img.shields.io/badge/.NET-8.0-blue?style=for-the-badge&logo=dotnet)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-lightgrey?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-blueviolet?style=for-the-badge&logo=microsoftsqlserver)
![Swagger](https://img.shields.io/badge/Swagger-darkgreen?style=for-the-badge&logo=swagger)
![Licença](https://img.shields.io/badge/Licen%C3%A7a-MIT-green.svg?style=for-the-badge)

**StockHive API** é uma API RESTful completa e robusta para gerenciamento de estoque, desenvolvida com as mais recentes tecnologias e melhores práticas do ecossistema .NET.

Este projeto foi construído do zero, aplicando conceitos avançados de arquitetura de software para garantir escalabilidade, manutenibilidade e segurança.

## Visão Geral

A API fornece endpoints para gerenciar o ciclo de vida completo de entidades essenciais para um sistema de estoque, incluindo:

* **Produtos e Categorias:** Com suporte a hierarquias (subcategorias).
* **Fornecedores e Locais de Estoque:** Gerenciamento de entidades base.
* **Inventário:** Controle preciso da quantidade de cada produto por local.
* **Movimentações de Estoque:** Rastreamento de todas as transações (compras, vendas, ajustes).

## Principais Tecnologias

* **.NET 8:** Utilizando a versão mais recente e performática da plataforma .NET.
* **ASP.NET Core:** Para a construção da API RESTful.
* **Entity Framework Core 8:** Como ORM para interação com o banco de dados, seguindo o padrão Code-First.
* **SQL Server on Azure:** Banco de dados relacional robusto e escalável na nuvem.
* **AutoMapper:** Para mapeamentos limpos e eficientes entre Entidades e DTOs.
* **Swagger/OpenAPI:** Para documentação interativa e testes de endpoints.
* **Asp.Versioning:** Para um versionamento de API limpo e profissional.

## Recursos e Funcionalidades de Destaque

Este projeto não é apenas um CRUD básico. Ele implementa uma série de padrões de arquitetura avançados:

* **Arquitetura Limpa (Simplificada):** O projeto é organizado com uma clara separação de preocupações (Models, DTOs, Controllers, Data, etc.).
* **Padrão DTO (Data Transfer Object):** Desacopla a API do modelo de dados interno, aumentando a segurança e a flexibilidade.
* **Soft Delete & Filtros Globais:** Nenhum dado é permanentemente deletado. Os registros são marcados como inativos e automaticamente excluídos de todas as consultas da aplicação via Filtros Globais do EF Core.
* **Auditoria Automática:** As datas de criação e atualização (`CreatedAt`, `UpdatedAt`) são gerenciadas automaticamente pelo `DbContext`, garantindo consistência.
* **Versionamento de API:** A API suporta versionamento via URL (`/api/v1/...`) para garantir que futuras mudanças não quebrem as integrações existentes.
* **Busca Avançada e Paginação:** Endpoints de listagem suportam filtragem por múltiplos campos, busca por range de datas e paginação, garantindo alta performance.
* **Tratamento de Hierarquia e Deleção em Cascata:** Lógica robusta no `CategoriesController` para gerenciar subcategorias e aplicar soft-delete em cascata de forma recursiva.
* **Atualização Parcial com `HTTP PATCH`:** Implementação correta do verbo `PATCH` para permitir atualizações eficientes de apenas alguns campos de um recurso.

## Como Executar o Projeto

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Acesso a um servidor SQL Server (local ou no Azure).
* Um editor de código como [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/) ou [VS Code](https://code.visualstudio.com/).

### Passos para Configuração

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/denilsonbslv/StockHive.git](https://github.com/denilsonbslv/StockHive.git)
    cd StockHive
    ```

2.  **Configure o Acesso Seguro (Secret Manager):**
    Este projeto utiliza o Secret Manager para armazenar a string de conexão de forma segura.
    * No terminal, na raiz do projeto, execute:
        ```bash
        dotnet user-secrets init
        dotnet user-secrets set "ConnectionStrings:DefaultConnection" "SUA_CONNECTION_STRING_AQUI"
        ```
    * *Substitua `SUA_CONNECTION_STRING_AQUI` pela sua string de conexão com o SQL Server.*

3.  **Confie no Certificado de Desenvolvimento:**
    Para rodar a aplicação em `https://` localmente, execute o seguinte comando em um terminal como **Administrador**:
    ```powershell
    dotnet dev-certs https --trust
    ```

4.  **Aplique as Migrações do Banco de Dados:**
    Este comando irá criar o banco de dados e todas as tabelas com base nos modelos definidos.
    ```powershell
    dotnet ef database update
    ```

5.  **Execute a Aplicação:**
    ```powershell
    dotnet run
    ```

6.  **Explore a API:**
    Abra seu navegador e acesse a URL indicada no terminal (geralmente `https://localhost:7149/swagger`). Você verá a documentação interativa do Swagger, pronta para testar todos os endpoints.

## Estrutura da API (Exemplos de Endpoints)

A API está versionada e segue os padrões RESTful.

| Verbo | Rota | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/v1/suppliers` | Lista, filtra e pagina os fornecedores. |
| `GET` | `/api/v1/suppliers/{id}` | Busca um fornecedor específico pelo seu ID. |
| `POST` | `/api/v1/suppliers` | Cria um novo fornecedor. |
| `PUT` | `/api/v1/suppliers/{id}` | Substitui completamente um fornecedor existente. |
| `PATCH` | `/api/v1/suppliers/{id}`| Atualiza parcialmente um fornecedor existente. |
| `DELETE`| `/api/v1/suppliers/{id}`| Realiza o soft-delete de um fornecedor. |

## Contribuindo

1.  Crie uma branch para sua feature ou correção: `git checkout -b feature/nova-funcionalidade`
2.  Faça commit das suas mudanças: `git commit -m "Descrição da mudança"`
3.  Envie para o repositório remoto: `git push origin feature/nova-funcionalidade`
4.  Abra um Pull Request e aguarde revisão.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).