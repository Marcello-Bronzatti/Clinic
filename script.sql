
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

-- Usu�rios
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL
);

-- Usu�rio para testes // admin, senha: admin
   INSERT INTO Users (Username, Password)
    VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=');

-- Inser��o de Pacientes
INSERT INTO Patients (Id, FullName, CPF, Email) VALUES
(NEWID(), 'Alice Costa', '12345678901', 'alice@teste.com'),
(NEWID(), 'Bruno Lima', '23456789012', 'bruno@teste.com');

-- Inser��o de Profissionais
INSERT INTO Professionals (Id, FullName, Specialty, CRM) VALUES
(NEWID(), 'Dr. Pedro Martins', 'Cardiologia', 'CRM-SP-001'),
(NEWID(), 'Dra. Fernanda Souza', 'Dermatologia', 'CRM-SP-002');

-- Inser��o de Agendamento (Exemplo v�lido)
INSERT INTO Appointments (Id, PatientId, ProfessionalId, ScheduledAt)
SELECT NEWID(), 
       (SELECT TOP 1 Id FROM Patients WHERE FullName = 'Alice Costa'),
       (SELECT TOP 1 Id FROM Professionals WHERE FullName = 'Dr. Pedro Martins'),
       '2025-06-13T10:00:00';


-- �ndices
CREATE INDEX idx_appointments_patient ON Appointments(PatientId, ProfessionalId, ScheduledAt);
CREATE INDEX idx_appointments_professional ON Appointments(ProfessionalId, ScheduledAt);
