using Amazon.Lambda.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        public APIGatewayProxyResponse GetOpportunity(APIGatewayProxyRequest request)
        {
            string oppId = request.PathParameters["id"];
            LambdaLogger.Log("get opportunity - db request " + oppId);

            string dbHost =  Environment.GetEnvironmentVariable("DB_HOST");
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword =  Environment.GetEnvironmentVariable("DB_PASSWORD");
            string dbName = Environment.GetEnvironmentVariable("DB_NAME");

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `Opportunity` WHERE `opportunityId` = @oppId;";
            cmd.Parameters.AddWithValue("@oppId", oppId);

            ArrayList opportunities = new ArrayList();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Opportunity opportunity = new Opportunity(reader.GetString("opportunityId"), reader.GetString("charityId"),reader.GetString("opportunityName"), reader.GetString("opportunityDescription"));
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

public APIGatewayProxyResponse SaveOpportunity(APIGatewayProxyRequest request) {

          string requestBody = request.Body;
          Opportunity o = JsonSerializer.Deserialize<Opportunity>(requestBody);
          LambdaLogger.Log("Saving Opportunity: " + o.Name);

          return new APIGatewayProxyResponse
          {
            Body = "Opportunity Saved",
            Headers = new Dictionary<string, string>
            { 
                { "Content-Type", "application/json" }, 
                { "Access-Control-Allow-Origin", "*" } 
            },
            StatusCode = 200,
          };
       }


    }

    public class Opportunity
    {
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Opportunity(string opportunityId, string charityId, string opportunityName, string opportunityDescription)
        {
            Id = opportunityId;
            CharityId = charityId;
            Name = opportunityName;
            Description = opportunityDescription;
        }

        public Opportunity() {}
    } 

    public class Request
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
    }
}
