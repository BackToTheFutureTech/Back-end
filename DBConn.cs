using MySql.Data.MySqlClient;
using System;
using Amazon.Lambda.Core;

namespace AwsDotnetCsharp
{
    public class DBConn
    {

        protected MySqlConnection sqlConnection;
        private string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        private string dbName = Environment.GetEnvironmentVariable("DB_NAME");

        public DBConn()
        {
            LambdaLogger.Log("Opening connection");
            this.sqlConnection = new MySqlConnection();
            this.sqlConnection.ConnectionString = $"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};";
            this.sqlConnection.Open();
        }

        public MySqlConnection getSqlConnection()
        {
            return this.sqlConnection;
        }

        public void closeConnection()
        {
            LambdaLogger.Log("Closing connection");
            if (this.sqlConnection.State == System.Data.ConnectionState.Open)
            {
                this.sqlConnection.Close();
            }
        }

    }
}
