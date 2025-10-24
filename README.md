# 📋 API de Gerenciamento de Tarefas - (ASP.NET Core 8 + JWT + SQL Server)

Este projeto é uma API RESTful desenvolvida com ASP.NET Core 8 para um desafio técnico. Ele implementa um sistema completo de gerenciamento de tarefas (estilo Kanban) com autenticação de usuário baseada em JSON Web Tokens (JWT).

Utiliza o Entity Framework Core com SQL Server para persistência de dados.

## ✅ Funcionalidades

### Autenticação (Auth)
- `POST /api/auth/register`: Regista um novo utilizador (com hash de senha).
- `POST /api/auth/login`: Autenticação de um utilizador e retorno de um token JWT.

### Utilizadores (Users)
- `GET /api/users`: Lista todos os utilizadores (necessita autenticação).
- `GET /api/users/{id}`: Obtém um utilizador por ID (necessita autenticação).

### Tarefas (Tasks) - *Endpoint Protegido*
- `GET /api/tasks`: Lista todas as tarefas *do utilizador autenticado*.
- `POST /api/tasks`: Cria uma nova tarefa (automaticamente definida como "A Fazer").
- `GET /api/tasks/{id}`: Obtém uma tarefa específica *do utilizador autenticado*.
- `DELETE /api/tasks/{id}`: Exclui uma tarefa *do utilizador autenticado*.
- `PUT /api/tasks/{id}/title`: Atualiza o título de uma tarefa.
- `PUT /api/tasks/{id}/status`: Atualiza o status de uma tarefa (ex: "ToDo", "InProgress", "Done").

## 🧱 Estrutura de Pastas

/TaskManagerApi 
├── Controllers/ 
│ ├── AuthController.cs # Endpoints de Login/Registo 
│ ├── TasksController.cs # CRUD de Tarefas 
│ └── UsersController.cs # CRUD de Utilizadores 
├── Data/ 
│ └── ApplicationDbContext.cs # Contexto do EF Core ├── DTOs/ 
│ ├── Auth/ # DTOs de Login, Registo, etc. 
│ └── Task/ # DTOs de Criação, Retorno, etc. 
├── Migrations/ # Migrações do EF Core 
├── Models/ 
│ ├── Enums/ 
│ │ └── TaskStatus.cs # Enum (ToDo, InProgress, Done) │ ├── Task.cs # Entidade Tarefa 
│ └── User.cs # Entidade Utilizador 
├── Properties/ 
│ └── launchSettings.json # Perfis de inicialização (http, https) 
├── Services/ 
│ ├── ITokenService.cs # Interface do serviço de token │ └── TokenService.cs # Implementação da geração de JWT 
├── appsettings.json # Configurações (Connection String, JWT Key) 
└── Program.cs # Configuração de serviços (DI, Auth, CORS, Swagger)


## 🛠️ Tecnologias Utilizadas

- **.NET 8** (SDK)
- **ASP.NET Core Web API**
- **Entity Framework Core 8** (para ORM)
- **SQL Server** (Provedor do EF Core)
- **Autenticação JWT** (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **BCrypt.Net** (para Hashing de senhas)
- **Swagger / OpenAPI** (para documentação e testes de API)
- **CORS** (para permitir acesso do frontend)

## 📦 Instalação e Execução

1.  **Clone o repositório**
    ```bash
    git clone https://github.com/devrayanco/task-manager-api.git
    cd TaskManagerApi
    ```

2.  **Configure a Connection String**
    * Abra o ficheiro `appsettings.json`.
    * Altere o valor de `ConnectionStrings:DefaultConnection` para apontar para o seu servidor SQL Server local.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=SEU_SERVIDOR;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

3.  **Restaure os pacotes e aplique as migrações**
    ```bash
    dotnet restore
    dotnet ef database update
    ```

4.  **Execute o projeto (usando o perfil HTTPS)**
    ```bash
    dotnet run
    ```

5.  **Acesse o Swagger**
    * O Swagger estará disponível no URL `https` indicado no terminal (ex: `https://localhost:7252/swagger`).

## 🧪 Testes (Swagger)

Esta API está configurada com o Swagger UI e inclui **suporte à autenticação JWT** (um dos diferenciais do desafio).

1.  Execute a API e abra o Swagger (ex: `https://localhost:7252/swagger`).
2.  Utilize o `POST /api/auth/register` para criar um utilizador.
3.  Utilize o `POST /api/auth/login` para obter um token JWT.
4.  No canto superior direito da página Swagger, clique em **"Authorize"**.
5.  Na janela pop-up, cole o seu token prefixado com `Bearer ` (ex: `Bearer eyJhbG...`).
6.  Agora pode testar todos os endpoints protegidos (como `GET /api/tasks`) diretamente pelo Swagger.

## 🔁 Exemplos de Requisições

**POST /api/auth/register**
```json
{
  "username": "testeuser",
  "email": "teste@email.com",
  "password": "Password123!"
}
```

**POST /api/tasks (Necessita Token "Bearer")**
```json
{
  "title": "Minha primeira tarefa"
}
```
Resposta esperada (201 Created)
```json
{
  "id": 1,
  "title": "Minha primeira tarefa",
  "status": "ToDo",
  "createdAt": "2025-10-24T10:30:00Z"
}
```



**PUT /api/tasks/1/status (Necessita Token "Bearer")**
```json
"InProgress"
```




)

## 🔒 Validações e Segurança
- Os campos de Registo (Username, Email, Password) são obrigatórios.
- O Email deve ser único no banco de dados.
- Todos os endpoints de Tasks e Users são protegidos com [Authorize].
- O acesso a dados de tarefas é restrito ao proprietário (o utilizador autenticado), garantindo que um utilizador não possa ver/editar tarefas de outro.
- O CORS está configurado para permitir pedidos apenas da aplicação web (ex: http://localhost:5173).






