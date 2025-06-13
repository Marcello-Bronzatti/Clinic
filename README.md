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

### 1. Clonar repositório

```bash
git clone https://github.com/seu-usuario/clinic-api.git
cd clinic-api
```

### 2. Configurar o banco de dados

Utilize o `docs/script.sql` para criar o banco de dados no SQL Server. Também existe um script opcional com dados de teste:

```sql
-- Script principal
docs/script.sql

-- Dados de teste (seed)
docs/seed-test-data.sql
```

### 3. Ajustar `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ClinicDb;Trusted_Connection=True;"
},
"Jwt": {
  "Key": "sua-chave-super-secreta",
  "Issuer": "ClinicAPI"
}
```

### 4. Rodar a aplicação

```bash
dotnet build
dotnet run
```

Acesse via:
```
https://localhost:5001/swagger
```

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
