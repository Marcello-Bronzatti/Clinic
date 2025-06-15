
# Clinic API 🩺

API RESTful para gerenciamento de agendamentos de consultas em uma clínica. Desenvolvida em .NET 7, com arquitetura DDD, Dapper e autenticação JWT.

---

## 🔧 Tecnologias

- .NET 7
- ASP.NET Core
- Dapper
- SQL Server 
- JWT Authentication
- Swagger
- NUnit (testes unitários)

---

## 📁 Estrutura de Pastas

```
📦Clinic.API
├── Domain/
├── Application/
├── Infrastructure/
├── WebApi/
├── Tests/
├── script.sql
└── docker-compose.yml
```

---

## 🚀 Como executar localmente

### 1. Pré-requisitos

- [.NET SDK 7.0](https://dotnet.microsoft.com/en-us/download) instalado (apenas se quiser rodar os testes)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução

---

### 2. Clonar o repositório

```bash
git clone https://github.com/marcello-bronzatti/clinic-api.git
cd clinic-api
```

---

### 3. Executar com Docker

Comando único:

```bash
docker-compose up --build
```

Esse comando irá:

- Subir o container do **SQL Server**
- Executar automaticamente o `script.sql` via serviço `db-init`
- Subir a API na porta `5000`

---

### 4. Acessar a API

Abra o navegador e acesse:

```
http://localhost:5000/swagger
```

---

## 🔐 Usuário de Teste

```text
Usuário: admin
Senha: admin
```

---

## ✅ Testes Automatizados

Para rodar os testes (fora do Docker):

```bash
dotnet test
```

Testes implementados para:

- AppointmentService
- PatientService
- ProfessionalService
- UserService

---

## 📜 Regras de Negócio

- ✅ Um paciente só pode ter 1 consulta por profissional por dia.
- ✅ Um profissional só pode atender uma consulta por horário.
- ✅ Consultas só são permitidas de segunda a sexta, das 08h às 18h.
- ✅ Cada consulta tem duração de 30 minutos.
- ✅ Agendamento só é permitido se houver horário disponível.

---

## 🐳 Docker & Docker Compose

### Arquitetura:

```
[Docker Compose]
│
├── clinic-api (porta 5000)
│   └── .NET 7 Web API
│
├── sqlserver (porta 1433)
│   └── Banco de dados SQL Server
│
└── db-init
    └── Executa script.sql automaticamente para criar e popular o banco
```

Os volumes e redes são definidos no `docker-compose.yml`.

---

## 📄 Autor

Desenvolvido por **Marcello Bronzatti**

[GitHub](https://github.com/marcello-bronzatti)
