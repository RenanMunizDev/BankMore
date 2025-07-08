using Dapper;
using Microsoft.Data.Sqlite;

namespace ContaCorrenteAPI.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static void ExecuteScripts(string connectionString)
        {
            var scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts");

            if (!Directory.Exists(scriptsPath))
            {
                throw new DirectoryNotFoundException($"Pasta de scripts não encontrada: {scriptsPath}");
            }

            var scriptFiles = Directory.GetFiles(scriptsPath, "*.sql").OrderBy(f => f);

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            foreach (var scriptFile in scriptFiles)
            {
                var scriptContent = File.ReadAllText(scriptFile);
                connection.Execute(scriptContent);
            }
        }
    }

}
