using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;

namespace AwsDotnetCsharp
{
    public class CharityAdminHandler
    {

        public APIGatewayProxyResponse PostOpportunity(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];

            DBConn dbconn = new DBConn();
            MySqlConnection connection = dbconn.getSqlConnection();
           
            try
            {
                Opportunity o = JsonSerializer.Deserialize<Opportunity>(request.Body);
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO `Opportunity`
                    ( opportunityName, opportunityDescription, 
                      charityId, taskType,
                      numVolunteers, opportunityDate, 
                      postcode, address1, address2, city,thumbnail) VALUES
                    (@name, @description, 
                     @charityId, @taskType, 
                     @numVolunteers, @date, 
                     @postcode,@address1,@address2,@city,@thumbnail);";
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
                cmd.Parameters.AddWithValue("@thumbnail", o.thumbnail);
                cmd.ExecuteNonQuery();
                mesg = "Opportunity Saved";
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
        public APIGatewayProxyResponse EditOpportunity(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];
            int opportunityId = Int32.Parse(request.PathParameters["opportunityId"]);

            DBConn dbconn = new DBConn();
            MySqlConnection connection = dbconn.getSqlConnection();
            
            try
            {
                Opportunity o = JsonSerializer.Deserialize<Opportunity>(request.Body);
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE `Opportunity`
                    SET opportunityName = @name, 
                        opportunityDescription = @description, 
                        taskType = @taskType,
                        numVolunteers = @numVolunteers, 
                        opportunityDate = @date, 
                        postcode = @postcode, 
                        address1 = @address1, 
                        address2 = @address2, 
                        city = @city,
                        thumbnail = @thumbnail
                    WHERE `opportunityId` = @opportunityId
                    AND `charityId` = @charityId;";
                cmd.Parameters.AddWithValue("@name", o.name);
                cmd.Parameters.AddWithValue("@description", o.description);
                cmd.Parameters.AddWithValue("@taskType", o.taskType);
                cmd.Parameters.AddWithValue("@numVolunteers", o.numVolunteers);
                cmd.Parameters.AddWithValue("@date", o.date);
                cmd.Parameters.AddWithValue("@postcode", o.postcode);
                cmd.Parameters.AddWithValue("@address1", o.address1);
                cmd.Parameters.AddWithValue("@address2", o.address2);
                cmd.Parameters.AddWithValue("@city", o.location);
                cmd.Parameters.AddWithValue("@thumbnail", o.thumbnail);
                cmd.Parameters.AddWithValue("@opportunityId", opportunityId);
                cmd.Parameters.AddWithValue("@charityId", charityId);
                cmd.ExecuteNonQuery();
                mesg = "Opportunity Updated";
                returnCode = 200;
            }
            catch (Exception err)
            {
                mesg = "Oops. Something went wrong. Failed to Update.";
                LambdaLogger.Log(err.ToString());
                returnCode = 500;
            }
            finally
            {
                dbconn.closeConnection();
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

        public APIGatewayProxyResponse DeleteOpportunity(APIGatewayProxyRequest request)

        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];
            int opportunityId = Int32.Parse(request.PathParameters["opportunityId"]);

            DBConn dbconn = new DBConn();
            MySqlConnection connection = dbconn.getSqlConnection();
            
            try
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"UPDATE `Opportunity`
                    SET isActive = false
                    WHERE `opportunityId` = @opportunityId
                    AND `charityId` = @charityId;";
                cmd.Parameters.AddWithValue("@charityId", charityId);
                cmd.Parameters.AddWithValue("@opportunityId", opportunityId);
                cmd.ExecuteNonQuery();
                mesg = "Opportunity Deleted";
                returnCode = 200;
            }
            
            catch (Exception err)
            {
                mesg = "Oops. Something went wrong. Failed to Delete.";
                LambdaLogger.Log(err.ToString());
                returnCode = 501;
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
