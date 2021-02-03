# MAD Days Application - Backend

This is the back end API of a Tech for Good MAD Days Application, built throughout the [Tech Returners](https://techreturners.com) Your Journey Into Tech course. It is consumed by a front end React application, available [here](https://github.com/BackToTheFutureTech/Front-end) and connects to an RDS MySQL Database.

The hosted version of the application is available here: [https://madday.herokuapp.com](https://madday.herokuapp.com).

### Technology Used

This project uses the following technology:

- Serverless Framework
- JavaScript (ES2015+)
- C#
- SQL
- Mysql library
- AWS Lambda and API Gateway
- AWS RDS
- ESLint

---
---
### Endpoints

The API exposes the following endpoints:

---

##### GET /opportunities

Responds with JSON containing all active volunteer opportunities in the Database.

---

##### GET /charities

Responds with JSON containing all charities in the Database.

---

##### POST /charities/:charityId/opportunities

Will create a new volunteer opportunity for an authorised charityId when sent a JSON payload in the format:

```json

{
        "name": "Seasonal clearout",
        "taskType": "Gardening",
        "numVolunteers": 5,
        "date": "2021-04-15T00:00:00",
        "postcode": "B8 9TH",
        "address1": "60 Grange Rd",
        "address2": "",
        "location": "Bolton",
        "description": "Make a child smile by helping us tidy up the outdoor garden and play area of ... hospice...",
        "thumbnail": "A url"
    }
```

##### POST /opportunities/:opportunityId/volunteers

Will regsiter a new volunteer or lead volunteer for a specific opportunity when sent a JSON payload in the format:
   
```json   
   
   {
        "volunteerName": "Wriggy",
        "email": "test@test.com",
        "mobile": "0777777777777",
        "groupNum": 10
}
```

---

##### PUT /charities/:charityId/opportunities/:opportunityId


Will update a volunteer opportunity for an authorised charityId when sent a JSON payload in the format:

```json

{
        "name": "An update",
        "taskType": "Gardening",
        "numVolunteers": 5,
        "date": "2021-04-15T00:00:00",
        "postcode": "B8 9TH",
        "address1": "60 Grange Rd",
        "address2": "",
        "location": "Bolton",
        "description": "Something new...",
        "thumbnail": "A url"
    }
```
    
---

##### DELETE /charities/:charityId/opportunities/:opportunityId


Will set a specified volunteer opportunity for an authorised charityId as inactive.  

    
---

---

### To build and deploy the API

#### Dependencies
1. A MySql database on [AWS](https://aws.amazon.com/) must be available.
2. [Serverless](https://www.serverless.com/framework/docs/getting-started/) must be installed and available with credentials set. 

    eg To install via npm     
    
        npm install -g serverless 

    To set credentials

        serverless config credentials --provider aws --key <key> --secret <secret>

3. [Dotnet Core](https://dotnet.microsoft.com/) must be installed and available

#### To build and deploy

1. Clone the repository  
2. To create the database tables and views run 

        mysql -u <user> -p -h <database instance endpoint> < ./sql/createTables.sql

   To insert some test data run

        mysql -u <user> -p -h <database instance endpoint> < ./sql/insertData.sql
 
3. To set the environment

    Create a config.dev.json file of the form config.example.json e.g.

        {
        "DB_HOST": "some-rds-endpoint",
        "DB_NAME": "some-database-name",
        "DB_USER": "some-database-user",
        "DB_PASSWORD": "some-user-password"
        }

4. To build the application run

        ./build.sh 

5. To deploy the lambda functions

        serverless deploy

---
