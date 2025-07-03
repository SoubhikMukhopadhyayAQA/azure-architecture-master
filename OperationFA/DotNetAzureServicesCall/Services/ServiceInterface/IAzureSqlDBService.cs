using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAzureServicesCall.Services.ServiceInterface
{
    public interface IAzureSqlDbService
    {
        bool CheckIfTableExists(string tableName);
        bool CheckLearnerDataExists(string correlationIdPrefix, string expectedState);
        bool CheckLrsJobDataExists(string lrsJobIdPrefix, string status);
        bool CleanupLearnerData(string correlationIdPrefix, string state);
        bool CleanupLrsJobData(string lrsJobIdPrefix, string status);
        bool CheckLrsAchievementJobDataExists(string lrsJobIdPrefix, string status);
        string LoadSqlQueryFromFile(string fileName);
        bool IsValidTableName(string tableName);
        bool InsertDataIntoTblAppointmentUpdate(int dynamicNumber);
        bool InserSpcialCharactertDataIntoTblAppointmentUpdate(int dynamicNumber);
        bool InsertNullDataIntoTblAppointmentUpdate();
    }
}
