-- Criação da tabela de Pacientes
CREATE TABLE Patients (
    Id TEXT PRIMARY KEY,
    FullName TEXT NOT NULL,
    CPF TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL UNIQUE
);

-- Criação da tabela de Profissionais
CREATE TABLE Professionals (
    Id TEXT PRIMARY KEY,
    FullName TEXT NOT NULL,
    Specialty TEXT NOT NULL,
    CRM TEXT NOT NULL UNIQUE
);

-- Criação da tabela de Agendamentos
CREATE TABLE Appointments (
    Id TEXT PRIMARY KEY,
    PatientId TEXT NOT NULL,
    ProfessionalId TEXT NOT NULL,
    ScheduledAt DATETIME NOT NULL,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id),
    FOREIGN KEY (ProfessionalId) REFERENCES Professionals(Id)
);

CREATE INDEX idx_appointments_patient ON Appointments(PatientId, ProfessionalId, ScheduledAt);
CREATE INDEX idx_appointments_professional ON Appointments(ProfessionalId, ScheduledAt);
