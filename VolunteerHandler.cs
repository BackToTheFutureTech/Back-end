using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;

namespace AwsDotnetCsharp
{
    public class VolunteerHandler
    {
        private string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        private string dbName = Environment.GetEnvironmentVariable("DB_NAME");

        public APIGatewayProxyResponse PostVolunteer(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string opportunityId = request.PathParameters["opportunityId"];

            DBConn dbconn = new DBConn();
            MySqlConnection connection = dbconn.getSqlConnection();

            try
            {
                Volunteer v = JsonSerializer.Deserialize<Volunteer>(request.Body);
                String id = Guid.NewGuid().ToString();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"START TRANSACTION; 
                INSERT INTO `Volunteer`
                    ( volunteerId, volunteerName, email, mobile) VALUES
                    (@id, @name, 
                     @email, @mobile); 
                INSERT INTO `Opportunity_Volunteer` (opportunityId, volunteerId, groupNum) VALUES
                    (@opportunityId, @id, @groupNum); COMMIT;";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@opportunityId", opportunityId);
                cmd.Parameters.AddWithValue("@name", v.volunteerName);
                cmd.Parameters.AddWithValue("@email", v.email);
                cmd.Parameters.AddWithValue("@mobile", v.mobile);
                cmd.Parameters.AddWithValue("@groupNum", v.groupNum);
                cmd.ExecuteNonQuery();
                mesg = "Signup Successful";
                returnCode = 200;
            }
            catch (Exception err)
            {
                mesg = "Oops. Something went wrong. Failed to sign up.";
                LambdaLogger.Log(err.ToString());
                returnCode = 500;
            }
            finally
            {
                connection.Close();
            };

            return new APIGatewayProxyResponse
            {
                Body = mesg,
                Headers = new Dictionary<string, string>
                {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
                },
                StatusCode = returnCode,
            };

        }

    }

}
