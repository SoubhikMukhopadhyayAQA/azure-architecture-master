using DotNetAzureServicesCall.Services.ServiceInterface;
using JetBrains.Annotations;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Xunit;

namespace DotNetAzureServicesCall.Services
{
    [UsedImplicitly]
    public class AzureSqlDbService : IAzureSqlDbService
    {
        private readonly string _connectionString;
        private readonly IFilePathProvider _filePathProvider;
        private readonly string _validTestDataSubFolder = "Int181";
        // ReSharper disable once ConvertToPrimaryConstructor
        public AzureSqlDbService(string connectionString, IFilePathProvider filePathProvider)
        {
            _connectionString = connectionString;
            _filePathProvider =
                filePathProvider ??
                throw new ArgumentNullException(nameof(filePathProvider));
        }

        public bool CheckIfTableExists(string tableName)
        {
            var query = @"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName AND TABLE_SCHEMA = 'dbo') 
                    SELECT 1 AS TablePresence 
                  ELSE 
                    SELECT 0 AS TablePresence;";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result) == 1;
        }
        public bool CheckLearnerDataExists(string correlationIdPrefix, string expectedState)
        {
            var query = @"
                SELECT COUNT(*) 
                FROM int_Learner
                WHERE CorrelationId LIKE @CorrelationIdPrefix + '%' AND State = @State;
                ";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CorrelationIdPrefix", correlationIdPrefix);
            command.Parameters.AddWithValue("@State", expectedState);

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }
        public bool CheckLrsJobDataExists(string lrsJobIdPrefix, string status)
        {
            var query = @"
                SELECT COUNT(*) 
                FROM [dbo].[int_LRSJob]
                WHERE Status LIKE @Status + '%' AND LRSJobId LIKE @lrsJobIdPrefix + '%';
            ";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@lrsJobIdPrefix", lrsJobIdPrefix);

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }
        public bool CleanupLearnerData(string correlationIdPrefix, string state)
        {
            var query = @"
                DELETE FROM [dbo].[int_Learner]
                WHERE CorrelationId LIKE @CorrelationIdPrefix AND State = @State;
                ";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CorrelationIdPrefix", $"{correlationIdPrefix}%");
                command.Parameters.AddWithValue("@State", state);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"Learner Table cleanup completed: {rowsAffected} row(s) deleted.");
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Learner Table cleanup failed: {ex.Message}");
                return false;
            }
        }
        public bool CleanupLrsJobData(string lrsJobIdPrefix, string status)
        {
            var query = @"
                DELETE FROM [dbo].[int_LRSJob]
                WHERE LRSJobId LIKE @LRSJobIdPrefix AND Status = @Status;
                ";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LRSJobIdPrefix", $"{lrsJobIdPrefix}%");
                command.Parameters.AddWithValue("@Status", status);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"LRS Job Table cleanup completed: {rowsAffected} row(s) deleted.");
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LRS Job Table cleanup failed: {ex.Message}");
                return false;
            }
        }
        public bool CheckLrsAchievementJobDataExists(string lrsJobIdPrefix, string status)
        {
            var query = @"
                SELECT COUNT(*) 
                FROM [dbo].[int_LrsAchievementBatchJob]
                WHERE Status LIKE @Status + '%' AND LrsAchievementJobId LIKE @lrsJobIdPrefix + '%';
            ";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@lrsJobIdPrefix", lrsJobIdPrefix);

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }
        private string LoadSqlQueryFromFile(string fileName)
        {
            try
            {
                string sourceFilePath = _filePathProvider.GetFilePath(_validTestDataSubFolder, fileName);
                Assert.True(File.Exists(sourceFilePath),
                    $"Expected file '{fileName}' does not exist at path '{sourceFilePath}'.");

                return File.ReadAllText(sourceFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading SQL file: " + ex.Message);
                return string.Empty;
            }
        }
        private bool IsValidTableName(string tableName)
        {
            return Regex.IsMatch(tableName, @"^[a-zA-Z0-9_]+$");
        }
        public bool InsertDataIntoTblAppointmentUpdate(int dynamicNumber)
        {
            var query = LoadSqlQueryFromFile("InsertDataInAppointmentUpdateQuery.sql");
            query = query.Replace("@dynamicNumber", dynamicNumber.ToString());

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                using var command = new SqlCommand(query, connection);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during insert: " + ex.Message);
                return false;
            }
        }
        public bool InserSpcialCharactertDataIntoTblAppointmentUpdate(int dynamicNumber)
        {
            var query = LoadSqlQueryFromFile("InsertSpecialCharacterDataInAppointmentUpdateQuery.sql");
            query = query.Replace("@dynamicNumber", dynamicNumber.ToString());

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                using var command = new SqlCommand(query, connection);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during insert: " + ex.Message);
                return false;
            }
        }
        public bool InsertNullDataIntoTblAppointmentUpdate()
        {
            var query = LoadSqlQueryFromFile("InsertNullDataInAppointmentUpdateQuery.sql");

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                using var command = new SqlCommand(query, connection);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during insert: " + ex.Message);
                return false;
            }
        }
    }
}
