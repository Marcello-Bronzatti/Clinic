# Clinic API 🩺

API RESTful para gerenciamento de agendamentos de consultas em uma clínica. Desenvolvida em ASP.NET Core 7, com arquitetura DDD, Dapper e autenticação JWT.

---

## 🔧 Tecnologias

- .NET 7
- ASP.NET Core
- Dapper
- SQL Server (compatível com PostgreSQL / MySQL)
- JWT Authentication
- Swagger
- nUnit (testes unitários)

---

## 📁 Estrutura de Pastas

```
📦Clinic.API
├── Domain/
├── Application/
├── Infrastructure/
├── API/
├── Tests/
└── docs/
```

---

## 🚀 Como executar

## 🚀 Como executar via Docker

 1. Pré-requisitos

- Docker Desktop instalado e em execução
- .NET SDK (apenas se quiser rodar testes)

---

 2. Clonar o repositório

```bash
git clone https://github.com/marcello-bronzatti/clinic-api.git
cd clinic-api

### 3. Ajustar `appsettings.json`

```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver;Database=ClinicDb;User=sa;Password=Clin1c@2024;TrustServerCertificate=True;"
  },


  "Jwt": {
    "Key": "MyUltraSecureSuperStrongJwtKey_1234567890!",
    "Issuer": "ClinicAuth",
    "Audience": "ClinicUsers"
  }
```
3. Subir os containers (API + SQL Server)

docker-compose up --build

Aguarde o build e a inicialização. Isso criará:

    sqlserver na porta 1433

    api na porta 5000
4. Acessar a API

Abra no navegador:

http://localhost:5000/swagger

---

## 🔐 Usuário de Teste

```text
Usuário: admin
Senha: admin
```

---

## ✅ Testes Automatizados

Para executar os testes:

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

## 📄 Autor

Desenvolvido por **Marcello Bronzatti**
