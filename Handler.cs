using Amazon.Lambda.Core;
using System.Collections;
using System.Collections.Generic;
using System;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
        public APIGatewayProxyResponse GetOpportunity(APIGatewayProxyRequest request)
        {
            string id = request.PathParameters["id"];
            LambdaLogger.Log("get opportunity " + id);

            ArrayList opportunities = new ArrayList();
            Opportunity o1 = new Opportunity("1", "Derian House", "Feed the Homeless", "Serve Food", 5, new DateTime(2021, 4, 15), "B8 9TH", "60 Grange Rd", "", "Bolton", "");
            Opportunity o2 = new Opportunity("2", "St Mary's", "Seasonal Clearout", "Gardening", 10, new DateTime(2021, 3, 30), "B8 9TH", "60 Grange Rd", "", "Crewe", "bla bla");

            if (id == "1")
            {
                opportunities.Add(o1);
            }
            else
            {
                opportunities.Add(o2);
            }

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
        public string Charity { get; set; }
        public string Name { get; set; }
        public string TaskType { get; set; }
        public int NumVolunteers { get; set; }
        public DateTime Date { get; set; }
        public string Postcode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public Opportunity(string opportunityId, string charity, string name, string taskType, int numVolunteers, DateTime date, string postcode, string address1, string address2, string location, string description)
        {
            Id = opportunityId;
            Charity = charity;
            Name = name;
            TaskType = taskType;
            NumVolunteers = numVolunteers;
            Date = date;
            Postcode = postcode;
            Address1 = address1;
            Address2 = address2;
            Location = location;
            Description = description;
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
