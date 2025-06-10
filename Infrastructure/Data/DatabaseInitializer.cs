using Dapper;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Data
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(string connectionString, string scriptPath)
        {
            if (!File.Exists(scriptPath)) return;

            var script = File.ReadAllText(scriptPath);
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            foreach (var commandText in script.Split("GO", System.StringSplitOptions.RemoveEmptyEntries))
            {
                using var command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }
        public static void SeedDefaultUser(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var checkUserCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = 'admin'", connection);
            var exists = (int)checkUserCmd.ExecuteScalar() > 0;
            if (exists) return;

            var password = "admin";
            var hash = HashPassword(password);

            var insertCmd = new SqlCommand("INSERT INTO Users (Id, Username, Password) VALUES (@Id, @Username, @Password)", connection);
            insertCmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
            insertCmd.Parameters.AddWithValue("@Username", "admin");
            insertCmd.Parameters.AddWithValue("@Password", hash);

            insertCmd.ExecuteNonQuery();
        }

        public static void SeedData(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            // SEED: Pacientes
            var patientId = Guid.NewGuid();
            connection.Execute(@"
                IF NOT EXISTS (SELECT 1 FROM Patients)
                BEGIN
                    INSERT INTO Patients (Id, FullName, CPF, Email)
                    VALUES (@Id, 'João da Silva', '12345678901', 'joao@email.com')
                END", new { Id = patientId });

            // SEED: Profissionais
            var professionalId = Guid.NewGuid();
            connection.Execute(@"
                IF NOT EXISTS (SELECT 1 FROM Professionals)
                BEGIN
                    INSERT INTO Professionals (Id, FullName, Specialty, CRM)
                    VALUES (@Id, 'Dra. Ana Paula', 'Cardiologista', 'CRM12345')
                END", new { Id = professionalId });

            // SEED: Agendamento
            var appointmentId = Guid.NewGuid();
            connection.Execute(@"
                IF NOT EXISTS (SELECT 1 FROM Appointments)
                BEGIN
                    INSERT INTO Appointments (Id, PatientId, ProfessionalId, ScheduledAt)
                    VALUES (@Id, @PatientId, @ProfessionalId, @ScheduledAt)
                END", new
            {
                Id = appointmentId,
                PatientId = patientId,
                ProfessionalId = professionalId,
                ScheduledAt = DateTime.UtcNow.AddDays(1)
            });
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }


}

