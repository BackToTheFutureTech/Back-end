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
    public class GetHandler
    {
        public APIGatewayProxyResponse GetAllOpportunities(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";

            DBConn dbconn = new DBConn();
            MySqlConnection connection = dbconn.getSqlConnection();

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
                        reader.GetString("description"),
                        reader.GetString("thumbnail"),
                        reader.GetInt32("numRegVolunteers")
                    );
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

        public APIGatewayProxyResponse GetAllCharities(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";

            DBConn dbconn = new DBConn();
            MySqlConnection connection = dbconn.getSqlConnection();

            ArrayList charities = new ArrayList();
            try
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT * FROM Charity";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charity charity = new Charity(
                        reader.GetString("charityId"),
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

}
