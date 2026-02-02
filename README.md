# 📚 Gerenciador de Livraria API

API REST desenvolvida em **ASP.NET Core (.NET)** para gerenciamento de livros, com foco em boas práticas de APIs, 
validações de dados e documentação.

---

## Tecnologias Utilizadas

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **C#**
- **LINQ**
- **Swagger (OpenAPI)**

---

## Funcionalidades

- Criar livros
- Listar livros
- Buscar livro por ID
- Buscar livros por **título ou autor** (com paginação)
- Atualizar informações de um livro (Atualização parcial)
- Excluir um livro
- Validações de regras de negócio
- Documentação automática via Swagger

---
## Estrutura do projeto

GerenciadorLivraria/
│
├── Controllers/
│   └── BooksController.cs
│
├── Data/
│   └── GerenciadorLivrariaDbContext.cs
│
├── Models/
│   └── Livro.cs
│
├── Dtos/
│   └── UpdateLivroDto.cs
│
├── Enums/
│   └── GeneroLivro.cs
│
├── Program.cs
├── appsettings.json
└── README.md

##  Modelo de Dados

### Livro

```csharp
public class Livro
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public GeneroLivro Genre { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

---

## Enum de Gêneros

```csharp
public enum GeneroLivro
{
    Undefined = 0,
    FiccaoCientifica = 1,
    Romance = 2,
    Misterio = 3,
    Fantasia = 4,
    Biografia = 5,
    Tecnologia = 6,
    Filosofia = 7,
    Autoajuda = 8,
    Satira = 9,
    Ficcao = 10,
    Suspense = 11
}
```

---

## DTO Utilizado

### UpdateLivroDto

Usado para atualização parcial do livro.

```csharp
public class UpdateLivroDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public GeneroLivro? Genre { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
}
```

---

## Endpoints

### Criar Livro

`POST /api/livros`

```json
{
  "title": "1984",
  "author": "George Orwell",
  "genre": 10,
  "price": 49.90,
  "stock": 20
}
``` 
ou

```json
{
  "title": "1984",
  "author": "George Orwell",
  "genre": "Ficcao",
  "price": 49.90,
  "stock": 20
}
```
> Obs: O campo `genre` pode ser enviado como **número ou como texto** desde que o valor exista em enum.
---

### Listar Livros

`GET /api/livros`

---
### Buscar Livro por ID
`GET /api/livros/{id}`
Retorna os dados completos de um livro específico

**Status possíveis:**
- `200 OK`: Livro encontrado
- `404 Not Found` : Livro não encontrado

### Buscar Livros (com Paginação)

`GET /api/livros/search?valor=George&page=1&pageSize=10`

- `valor`: título ou autor
- `page`: página atual
- `pageSize`: quantidade de itens por página

Retorna apenas:
- Título
- Gênero
- Preço
- Estoque

---

### Atualizar Livro

`PUT /api/livros/{id}`

```json
{
  "price": 59.90,
  "stock": 15
}
```
- Apenas os campos enviados no corpo da requisição serão atualizados.
Campos omitidos permanecem com seus valores atuais.
---

### Excluir Livro

`DELETE /api/livros/{id}`

---
# Status Codes utilizados

| Código |       Descrição                |
|--------|--------------------------------|
|  200   | Requisição bem-sucedida        |
|  201   | Recurso criado                 |
|  204   | Operação realizada sem retorno |
|  400   | Erro de validação              |
|  404   | Recurso não encontrado         |
|  409   | Conflito de dados              |
|  500   | Erro interno                   |

---

## Documentação 

A API é documentada utilizando:

- Comentários XML (`///`)
- Swagger (OpenAPI) para visualização e testes interativos

Disponível em:

```
https://localhost:{porta}/swagger
```

A documentação contém:

- Descrição dos endpoints  
- Exemplos de request  
- Parâmetros de query  
- Modelos de dados  

---

## Como executar o projeto

### Pré-requisitos

- .NET 8 SDK  
- PostgreSQL  
- Visual Studio ou VS Code  

### Passos

1. Clone o repositório:
```
git clone https://github.com/DevLucasP/Gerenciador-livraria.git
```

2. Configure a string de conexão no `appsettings.json`  
3. Execute as migrations (se aplicável)  
4. Rode o projeto:
```
dotnet run
```
---

## Possíveis Evoluções

- Autenticação e autorização
- Relacionamento Livro x Autor
- Paginação avançada
- Cache
- Versionamento de API
- Testes unitários
---

## Autor

Lucas Pacheco

Projeto desenvolvido para estudo e evolução em **Backend .NET / APIs REST**
