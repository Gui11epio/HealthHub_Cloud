# Health Hub

### 👥 Nome e RM dos Integrantes

- Guilherme Camasmie Laiber de Jesus – RM554894

- Fernando Fernandes Prado – RM557982

- Pedro Manzo Yokoo – RM556115

### 📌 Descrição do Projeto

O Health Hub é uma API desenvolvida em ASP.NET Core que fornece funcionalidades para acompanhamento de bem-estar, gestão de usuários, respostas de questionários, comunicação com IA e suporte corporativo à saúde mental.

### 📌 Arquitetura do Projeto

A aplicação implementa operações básicas de CRUD (Create, Read, Update, Delete), segue uma arquitetura em camadas (Controllers, Application, Domain, Infrastructure), segue os príncipios de DDD e Clean Code.

Com o objetivo de deixar a aplicação mais organizada e destribuir as responsabilidades

## 🚀 Rotas Disponíveis

### 📍 Questionario (V1)
- `GET /api/v1/Questionario`  
  Retorna todos os questionários cadastrados.

- `GET /api/v1/Questionario/{id}`  
  Retorna um questionário específico pelo id.

- `GET /api/v1/Questionario/pagina`  
  Retorna questionários por meio de páginas.

- `POST /api/v1/Questionario`  
  Cria um novo questionário. Requer um corpo com os dados do questionário.

- `DELETE /api/v1/Questionario/{id}`  
  Deleta um questionário pelo id.


### 📍 Usuário (V1)

- `GET /api/v1/Usuario/{id}`
Obtém um usuário por ID

- `PUT /api/v1/Usuario/{id}`
Atualiza um usuário existente

- `DELETE /api/v1/Usuario/{id}`
Remove um usuário

- `GET /api/v1/Usuario/email/{email}`
Obtém um usuário por email

- `GET /api/v1/Usuario`
Obtém todos os usuários

- `POST /api/v1/Usuario`
Cria um novo usuário

- `GET /api/v1/Usuario/pagina`
Obtém usuários paginados

- Pode ser usada pelo Postman, apenas use o link junto com **rotas** disponíveis acima.

## 🚀 Rota dos Health Checks
- `/health`
  Vai mostrar o estado de tudo

- `/health/live`
  Vai mostrar o estado da Aplicação apenas


## 🛠️ Tecnologias Utilizadas

- [.NET 6 / ASP.NET Core](https://dotnet.microsoft.com/)
- C#
- Entity Framework Core
- Swagger (OpenAPI) para documentação
- Visual Studio 2022
- Oracle DataBase
- AutoMapper
- Migrations
- DataAnnotations
- Pagination
- HATEOAS
- JWT
- Health Check
- xUnit
- Versionamento de API

## ▶️ Instruções de Execução WebApp

1. **Abra o Azure CLI**
   
2. **Clone o Repositório**
   ```bash
   git clone https://github.com/Gui11epio/HealthHub_Cloud.git
   ````
3. **Entre na raiz do projeto**
   ```bash
   cd HealthHub_Cloud/
   ```
4. **Rode o arquivo script_infra.sh, localizado dentro da pasta /scripts em Health-Hub**

   [script_infra.sh](https://github.com/user-attachments/files/23666601/script_infra.sh)
 
5. **Após rodar o arquivo entre no webapp para testar no Postman** **Os camanhos para serem utilizados estão no inicio do documento**
   ```bash
   https://healthhub-app.azurewebsites.net
   ````



## 📬JSON de Teste para o Swagger

- Questionário
  
```bash
{
  "usuarioId": 1,
  "nivelEstresse": 6,
  "qualidadeSono": 4,
  "ansiedade": 5,
  "sobrecarga": 2
}
```

#

- Usuário
```bash
{
  
  "emailCorporativo": "guilherme@gmail.com",
  "nome": "Guilherme",
  "senha": "GuiTatu0203!",
  "tipo": "ADMIN"

}
```
🔤 Tipo deve conter:

- Tipo: "ADMIN" ou "FUNCIONARIO"


## Diagrama CI/CD do Projeto

<img width="1321" height="621" alt="Diagrama" src="https://github.com/user-attachments/assets/17ebe5fc-2567-413b-934c-c6d581c6f509" />





  



   
