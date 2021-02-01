using Amazon.Lambda.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using MySql.Data.MySqlClient;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

// ToDo handle nulls; deal with file uploads

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

        public APIGatewayProxyResponse PostOpportunity(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];
            LambdaLogger.Log(request.Body);
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

        public APIGatewayProxyResponse EditOpportunity(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];
            int opportunityId = Int32.Parse(request.PathParameters["opportunityId"]);

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
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

        public APIGatewayProxyResponse DeleteOpportunity(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string charityId = request.PathParameters["charityId"];
            int opportunityId = Int32.Parse(request.PathParameters["opportunityId"]);

            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
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

        public APIGatewayProxyResponse PostVolunteer(APIGatewayProxyRequest request)
        {
            int returnCode = 1;
            string mesg = "";
            string opportunityId = request.PathParameters["opportunityId"];
            
            MySqlConnection connection = new MySqlConnection($"server={dbHost};user id={dbUser};password={dbPassword};port=3306;database={dbName};");
            connection.Open();
            try
            {
                Volunteer v = JsonSerializer.Deserialize<Volunteer>(request.Body);
                String id = Guid.NewGuid().ToString();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"START TRANSACTION; 
                INSERT INTO `Volunteer`
                    ( volunteerId volunteerName, email, mobile) VALUES
                    (@id, @name, 
                     @email, @mobile); 
                INSERT INTO `Opportunity_Volunteer` (opportunityId, volunteerId) VALUES
                    (@opportunityId, @id); COMMIT;";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@opportunityId", opportunityId);
                cmd.Parameters.AddWithValue("@name", v.volunteerName);
                cmd.Parameters.AddWithValue("@email", v.email);
                cmd.Parameters.AddWithValue("@mobile", v.mobile);
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
        public string thumbnail { get; set; }
        public int numRegVolunteers { get; set; }

        public Opportunity(int Id, string Charity, string Name, string TaskType, int NumVolunteers, string Date, string Postcode, string Address1, string Address2, string City, string Description, string Thumbnail, int NumRegVolunteers)
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
            thumbnail = Thumbnail;
            numRegVolunteers = NumRegVolunteers;
        }

        public Opportunity() { }
    }
    public class Charity
    {
        public string charityId { get; set; }
        public string charityName { get; set; }
        public string imageUrl { get; set; }
        public string charityDescription { get; set; }

        public Charity(string Id, string Name, string ImgURL, string Description)
        {
            charityId = Id;
            charityName = Name;
            imageUrl = ImgURL;
            charityDescription = Description;
        }
        public Charity() { }
    }

    public class Volunteer
    {
        public string volunteerId { get; set; }
        public string volunteerName { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }

        public Volunteer(string Id, string Name, string Email, string Mobile)
        {
            volunteerId = Id;
            volunteerName = Name;
            email = email;
            mobile = mobile;
        }
        public Volunteer() { }
    }

    public class Request
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
    }
}
