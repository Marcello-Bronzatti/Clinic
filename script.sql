-- Pacientes
CREATE TABLE Patients (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    CPF NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE
);

-- Profissionais
CREATE TABLE Professionals (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Specialty NVARCHAR(100) NOT NULL,
    CRM NVARCHAR(50) NOT NULL UNIQUE
);

-- Agendamentos
CREATE TABLE Appointments (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PatientId UNIQUEIDENTIFIER NOT NULL,
    ProfessionalId UNIQUEIDENTIFIER NOT NULL,
    ScheduledAt DATETIME NOT NULL,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (ProfessionalId) REFERENCES Professionals(Id)
);

-- Usuários
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL
);

-- Índices
CREATE INDEX idx_appointments_patient ON Appointments(PatientId, ProfessionalId, ScheduledAt);
CREATE INDEX idx_appointments_professional ON Appointments(ProfessionalId, ScheduledAt);
