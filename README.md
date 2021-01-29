# MAD Days Application - Backend

This is the back end API of a Tech for Good MAD Days Application, built throughout the [Tech Returners](https://techreturners.com) Your Journey Into Tech course. It is consumed by a front end React application, available [here](https://github.com/BackToTheFutureTech/Front-end) and connects to an RDS MySQL Database.

The hosted version of the application is available here: [https://madday.herokuapp.com](https://madday.herokuapp.com).

### Technology Used

This project uses the following technology:

- Serverless Framework
- JavaScript (ES2015+)
- SQL
- Mysql library
- AWS Lambda and API Gateway
- AWS RDS
- ESLint

### Endpoints

The API exposes the following endpoints:

---

##### GET /opportunities

[https://r892sqdso9.execute-api.eu-west-2.amazonaws.com/opportunities](https://r892sqdso9.execute-api.eu-west-2.amazonaws.com/opportunities)

Responds with JSON containing all active volunteer opportunities in the Database.

---

---

##### GET /charities

[https://r892sqdso9.execute-api.eu-west-2.amazonaws.com/charities](https://r892sqdso9.execute-api.eu-west-2.amazonaws.com/charities)

Responds with JSON containing all charities in the Database.

---

##### POST /opportunity

[https://r892sqdso9.execute-api.eu-west-2.amazonaws.com/opportunities/{charityId}](https://r892sqdso9.execute-api.eu-west-2.amazonaws.com/opportunities/{charityId})

Will create a new volunteer opportunity when sent a JSON payload with an authorised charityId in the format:

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
        "description": "Make a child smile by helping us tidy up the outdoor garden and play area of ... hospice..."
    }

```
