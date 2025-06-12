-- Pacientes
CREATE TABLE Patients (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName NVARCHAR(100) NOT NULL,
    CPF NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE
);

-- Profissionais
CREATE TABLE Professionals (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(100) NOT NULL,
    CRM NVARCHAR(50) NOT NULL UNIQUE
);

-- Agendamentos
CREATE TABLE Appointments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PatientId UNIQUEIDENTIFIER NOT NULL,
    ProfessionalId UNIQUEIDENTIFIER NOT NULL,
    ScheduledAt DATETIME NOT NULL,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (ProfessionalId) REFERENCES Professionals(Id)
);

-- Usuários
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL
);

-- Usuário para testes // admin, senha: admin
   INSERT INTO Users (Username, Password)
    VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=');

-- Índices
CREATE INDEX idx_appointments_patient ON Appointments(PatientId, ProfessionalId, ScheduledAt);
CREATE INDEX idx_appointments_professional ON Appointments(ProfessionalId, ScheduledAt);
