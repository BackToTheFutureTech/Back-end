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
            int returnCode = 1;
            string mesg = "";

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
            ArrayList opportunities = new ArrayList();
            try
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM `V_allOpportunities`";

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
                mesg = JsonSerializer.Serialize(opportunities);
                returnCode = 200;
            }
            catch (Exception err)
            {
                mesg = "Oops. Something went wrong. Failed to read data.";
                LambdaLogger.Log(err.ToString());
                returnCode = 500;
            }
            finally
            {
                connection.Close();
            }

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

        public APIGatewayProxyResponse PostOpportunity(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
            try
            {
                Opportunity o = JsonSerializer.Deserialize<Opportunity>(request.Body);
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO `Opportunity`
                    ( opportunityName, opportunityDescription, 
                      charityId, taskType,
                      numVolunteers, opportunityDate, 
                      postcode, address1, address2, city) VALUES
                    (@name, @description, 
                     @charityId, @taskType, 
                     @numVolunteers, @date, 
                     @postcode,@address1,@address2,@city);";
                cmd.Parameters.AddWithValue("@name", o.name);
                cmd.Parameters.AddWithValue("@charityId", charityId);
                cmd.Parameters.AddWithValue("@description", o.description);
                cmd.Parameters.AddWithValue("@taskType", o.taskType);
                cmd.Parameters.AddWithValue("@numVolunteers", o.numVolunteers);
                cmd.Parameters.AddWithValue("@date", o.date);
                cmd.Parameters.AddWithValue("@postcode", o.postcode);
                cmd.Parameters.AddWithValue("@address1", o.address1);
                cmd.Parameters.AddWithValue("@address2", o.address2);
                cmd.Parameters.AddWithValue("@city", o.location);
                cmd.ExecuteNonQuery();
                mesg = "Opportunity Saved for " + charityId;
                returnCode = 200;
            }
            catch (Exception err)
            {
                mesg = "Oops. Something went wrong. Failed to save.";
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

        public APIGatewayProxyResponse GetAllCharities(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
            ArrayList charities = new ArrayList();
            try
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM Charity";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charity charity = new Charity(
                        reader.GetInt32("charityId"),
                        reader.GetString("charityName"),
                        reader.GetString("imageUrl"),
                        reader.GetString("charityDescription"));
                    charities.Add(charity);
                }
                mesg = JsonSerializer.Serialize(charities);
                returnCode = 200;
            }
            catch (Exception err)
            {
                mesg = "Oops. Something went wrong. Failed to read data.";
                LambdaLogger.Log(err.ToString());
                returnCode = 500;
            }
            finally
            {
                connection.Close();
            }

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

        public Opportunity(int Id, string Charity, string Name, string TaskType, int NumVolunteers, string Date, string Postcode, string Address1, string Address2, string City, string Description)
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
    public class Charity
    {
        public int charityId { get; set; }
        public string charityName { get; set; }
        public string imageUrl { get; set; }
        public string charityDescription { get; set; }

        public Charity(int Id, string Name, string ImgURL, string Description)
        {
            charityId = Id;
            charityName = Name;
            imageUrl = ImgURL;
            charityDescription = Description;
        }
        public Charity() { }
    }

    public class Request
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
    }
}
