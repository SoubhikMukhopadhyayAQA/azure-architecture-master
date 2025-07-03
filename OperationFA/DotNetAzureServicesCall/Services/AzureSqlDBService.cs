using DotNetAzureServicesCall.Services.ServiceInterface;
using JetBrains.Annotations;
using System.Data.SqlClient;

namespace DotNetAzureServicesCall.Services
{
    [UsedImplicitly]
    public class AzureSqlDbService : IAzureSqlDbService
    {
        private readonly string _connectionString;
        // ReSharper disable once ConvertToPrimaryConstructor
        public AzureSqlDbService(string connectionString)
        {
            _connectionString = connectionString;
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
    }
}
