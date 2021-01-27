using Amazon.Lambda.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

// ToDo date conversion, handle Nulls //

namespace AwsDotnetCsharp
{
    public class Handler
    {
        private string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        private string dbName = Environment.GetEnvironmentVariable("DB_NAME");

        public APIGatewayProxyResponse GetAllOpportunities(APIGatewayProxyRequest request)
        {
            //LambdaLogger.Log("get all active opportunities");

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `V_allOpportunities`";
            ArrayList opportunities = new ArrayList();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Opportunity opportunity = new Opportunity(
                    reader.GetInt32("id"), 
                    reader.GetString("charity"),
                    reader.GetString("name"), 
                    reader.GetString("taskType"), 
                    reader.GetInt32("numVolunteers"), 
                    reader.GetString("date"), 
                    reader.GetString("postcode"), 
                    reader.GetString("address1"), 
                    reader.GetString("address2"), 
                    reader.GetString("city"), 
                    reader.GetString("description"));
                opportunities.Add(opportunity);
            }

            connection.Close();

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(opportunities),
                Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
                StatusCode = 200,
            };
        }
        
        public APIGatewayProxyResponse PostOpportunity(APIGatewayProxyRequest request)
        {
            string charityId = request.PathParameters["charityId"];
            string requestBody = request.Body;
            Opportunity o = JsonSerializer.Deserialize<Opportunity>(requestBody);
            //LambdaLogger.Log("Saving Opportunity: " + o.name);

             MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
            try {
            string sql = @"INSERT INTO `Opportunity`
                    ( opportunityName, opportunityDescription, 
                      charityId, taskType,
                      numVolunteers, opportunityDate, 
                      postcode, address1, address2, city) VALUES
                    ('A new opportunity', 'bla bla bla bla', 
                    'e95d69d9-5c9d-11eb-83f0-06358a409ac0', 'Serve Food', 
                     20, '2021-03-10', 
                     'BL2 24D','60 Grange Rd',' ','Bolton');";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                LambdaLogger.Log(err.ToString());
            }
            connection.Close();
            int statusCode = 200;
            return new APIGatewayProxyResponse
            {
                Body = "Opportunity Saved for " +charityId,
                Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }
            },
                StatusCode = statusCode,
            };
        }


    }


    public class Opportunity
    {
        public int id { get; set; }
        public string charity { get; set; }
        public string name { get; set; }
        public string taskType { get; set; }
        public int numVolunteers { get; set; }
        public string date { get; set; }
        public string postcode { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string location { get; set; }
        public string description { get; set; }

        public Opportunity (int Id, string Charity, string Name, string TaskType, int NumVolunteers, string Date, string Postcode, string Address1, string Address2, string City, string Description)
        {
            id = Id;
            charity = Charity;
            name = Name;
            taskType = TaskType;
            numVolunteers = NumVolunteers;
            date = Date;
            postcode = Postcode;
            address1 = Address1;
            address2 = Address2;
            location = City;
            description = Description;
        }

        public Opportunity() { }
    }

    public class Request
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
    }
}
