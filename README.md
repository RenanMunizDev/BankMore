
# 💰 BankMore - Sistema de Transferência Bancária

BankMore é um sistema bancário simplificado, desenvolvido em .NET 8 com foco em boas práticas de arquitetura, mensageria, segurança e testes. Ele simula funcionalidades de uma conta corrente e transferências entre contas, com autenticação JWT, validação de CPF e suporte a Docker e CI/CD.

## 📁 Estrutura do Projeto

```
BankMore/
├── src/
│   ├── ContaCorrenteAPI/         # API responsável pela gestão de contas
│   ├── ContaCorrenteAPI.Tests/   # Testes unitários da API de contas
│   ├── TransferenciaAPI/         # API responsável por transferências
│   ├── TransferenciaAPI.Tests/   # Testes unitários da API de transferências
├── docker-compose.yml            # Orquestração dos serviços via Docker
└── README.md
```

---

## 🚀 Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core (SQLite)**
- **MediatR (CQRS)**
- **FluentValidation**
- **JWT Authentication**
- **Dapper**
- **Swagger (OpenAPI)**
- **Docker e Docker Compose**
- **xUnit**
- **Kafka** (⚠️ _não implementado ainda)

---

## 🔐 Funcionalidades

### ContaCorrenteAPI

- Criar conta corrente com validação de CPF
- Efetuar login e gerar JWT
- Consultar saldo
- Inativar conta (soft delete)
- Atualizar e persistir movimentações
- JWT Token obrigatório para operações protegidas

### TransferenciaAPI

- Efetuar transferências entre contas usando o número da conta
- Persistência com idempotência
- Comunicação com ContaCorrenteAPI via HttpClient
- JWT obrigatório
- Fallback para valores inválidos
- Organização por CQRS + validações

---

## 🐳 Como executar com Docker

### Pré-requisitos:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker + Docker Compose](https://www.docker.com/)

### Passos para rodar o projeto:

```bash
# Clone o repositório
git clone https://github.com/renanmunizdev/BankMore.git
cd BankMore

# Suba os containers com Docker Compose
docker-compose up --build
```

### Acesse no navegador:

- ContaCorrenteAPI: [http://localhost:5001/swagger](http://localhost:5001/swagger)
- TransferenciaAPI: [http://localhost:5002/swagger](http://localhost:5002/swagger)

---

## 🧪 Rodando os testes

```bash
# Acesse a pasta do projeto
cd src/ContaCorrenteAPI.Tests
dotnet test

cd ../TransferenciaAPI.Tests
dotnet test
```

---

## 🔄 CI/CD

Você pode configurar um pipeline de CI/CD com GitHub Actions, GitLab CI ou Azure Pipelines. Sugestões:

- Build da solution
- Execução dos testes
- Docker build + push (ex: Docker Hub)
- Deploy (Kubernetes, Azure Web App, etc.)

---

## ⚠️ Observações

- A implementação de mensageria com Kafka foi planejada, mas não concluída por limite de tempo. O projeto já está preparado para integração futura com KafkaFlow.
- A solução está em SQLite para facilitar testes, mas é facilmente adaptável para SQL Server ou PostgreSQL.

---

## 👨‍💻 Autor

Desenvolvido por **Renan Muniz**  
Contato: [LinkedIn](https://www.linkedin.com/in/renanmuniz86) · [GitHub](https://github.com/renanmunizdev)

---

## 📄 Licença

Este projeto está sob a licença MIT. Sinta-se à vontade para usar e contribuir!
