using Microsoft.Data.Sqlite;

namespace Infrastructure.Data
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(string connectionString, string scriptPath)
        {
            if (!File.Exists(scriptPath)) return;

            var script = File.ReadAllText(scriptPath);
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = script;
            command.ExecuteNonQuery();
        }
    }
}
