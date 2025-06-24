# StockHive

StockHive é uma API RESTful de gerenciamento de estoque desenvolvida em ASP.NET Core (.NET 8) e Entity Framework Core, utilizando SQL Server como banco de dados.

## Visão Geral

A solução é organizada em camadas:

- **Models**: contém as entidades que representam as tabelas do banco de dados (Category, Product, Supplier, Location, Inventory, ProductAttribute, StockMovement).
- **Data**: inclui o `AppDbContext`, responsável pela configuração do Entity Framework Core, aplicando soft deletes, filtros globais e auditoria automática.
- **Interfaces**: define contratos, como `IAuditable`, para implementar recursos de auditoria (CreatedAt, UpdatedAt, DeletedAt).
- **Controllers**: pasta dedicada aos controllers da API (endpoints HTTP).

## Principais Funcionalidades

- **CRUD** completo para produtos, categorias, fornecedores, locais, atributos e movimentações de estoque.
- **Auditoria automática**: gravação de datas de criação e atualização via `IAuditable` e `SaveChangesAsync` sobrescrito.
- **Soft delete**: entidades marcadas como deletadas não são retornadas nas consultas, graças aos filtros globais configurados em `OnModelCreating`.
- **Migrations**: uso de EF Core Migrations para versionar o schema do banco.
- **Swagger** para documentação interativa da API em tempo de desenvolvimento.

## Requisitos

- .NET 8 SDK
- SQL Server

## Configuração

1. Atualize a string de conexão em `appsettings.json` (ou `appsettings.Development.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=StockHive;Trusted_Connection=True;"
   }
   ```
2. Execute as migrations e atualize o banco:
   ```powershell
   dotnet ef database update
   ```
3. Inicie a aplicação:
   ```powershell
   dotnet run
   ```
4. Acesse o Swagger em `https://localhost:{porta}/swagger` para explorar os endpoints.

## Estrutura de Pastas

```
StockHive/
├── Controllers/          # Endpoints da API
├── Data/                 # DbContext e configurações do EF Core
├── Interfaces/           # Contratos e interfaces (auditoria)
├── Models/               # Entidades do domínio
├── Migrations/           # Arquivos de migração do EF Core
├── Program.cs            # Ponto de entrada e configuração da aplicação
├── appsettings.json      # Configurações gerais
└── README.md             # Documentação do projeto
```

## Contribuindo

1. Crie uma branch para sua feature ou correção: `git checkout -b feature/nova-funcionalidade`
2. Faça commit das suas mudanças: `git commit -m "Descrição da mudança"`
3. Envie para o repositório remoto: `git push origin feature/nova-funcionalidade`
4. Abra um Pull Request e aguarde revisão.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
