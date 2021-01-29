USE MADdays;

SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE Picture;
TRUNCATE TABLE Comment;
TRUNCATE TABLE Opportunity_Volunteer;
TRUNCATE TABLE Opportunity;
TRUNCATE TABLE Volunteer; 
TRUNCATE TABLE Charity;
TRUNCATE TABLE TaskImage;

SET FOREIGN_KEY_CHECKS = 1;

INSERT INTO Charity 
    (charityId, charityName, charityDescription, imageUrl) 
VALUES 
("963387ca-5c9d-11eb-83f0-06358a409ac0", "NSPCC","bla bla bla","https://github.com/BackToTheFutureTech/Front-end/blob/main/src/Assets/SortClothing.jpg?raw=true"),
("e95d69d9-5c9d-11eb-83f0-06358a409ac0", "St Mary's","bla bla bla","https://github.com/BackToTheFutureTech/Front-end/blob/main/src/Assets/SortClothing.jpg?raw=true"),
("e9ded807-5c9d-11eb-83f0-06358a409ac0", "Derrian House","bla bla bla", "https://github.com/BackToTheFutureTech/Front-end/blob/main/src/Assets/SortClothing.jpg?raw=true")
; 

INSERT INTO TaskImage 
    (taskType) VALUES 
    ("Wrap Presents"),
    ("Gardening"),
    ("Sort Clothes"),
    ("Serve Food"),
    ("Other")
;

INSERT INTO Opportunity 
    ( opportunityName, opportunityDescription, charityId, taskType,numVolunteers,opportunityDate, postcode, address1, address2, city) VALUES 
    ("Feeding the homeless", "bla bla bla bla", "e95d69d9-5c9d-11eb-83f0-06358a409ac0", "Serve Food", 20, "2021-03-10", "BL2 24D","60 Grange Rd"," ","Bolton"),
    ("Seasonal tidy up", "bla bla bla bla", "e95d69d9-5c9d-11eb-83f0-06358a409ac0", "Gardening",
     10, "2021-03-10", "C24 3TR", "600 Grange Rd"," ", "Crewe"),
    ("Christmas Cheer", "bla bla bla bla", "e9ded807-5c9d-11eb-83f0-06358a409ac0", "Wrap Presents",
     5, "2021-03-19", "BL1 1SD","36 Windermere Rd"," ", "Bolton"),
     ("For the children", "bla bla bla bla", "e9ded807-5c9d-11eb-83f0-06358a409ac0", "Wrap Presents", 10, "2021-04-15", "BL1 1SD","36 Windermere Rd"," ", "Bolton"),
      ("Help the Homeless", "bla bla bla bla", "e9ded807-5c9d-11eb-83f0-06358a409ac0", "Sort Clothes", 10, "2021-04-22", "BL1 1SD","36 Windermere Rd"," ", "Bolton")
;

INSERT INTO Volunteer 
    (volunteerId, volunteerName, email, mobile) VALUES 
    ( "85f1966e-5c9e-11eb-83f0-06358a409ac0", "John Doe", "bla@blabla.co.uk", "077777777777" ),
    ( "867b5397-5c9e-11eb-83f0-06358a409ac0", "Jane Doe", "bla@blabla.co.uk", "077777777777" )
;

INSERT INTO Opportunity_Volunteer 
    (opportunityId, VolunteerId) VALUES 
    (1, "85f1966e-5c9e-11eb-83f0-06358a409ac0" ),
    (1, "867b5397-5c9e-11eb-83f0-06358a409ac0" );

INSERT INTO Comment 
    (charityId, comment) VALUES 
    ("963387ca-5c9d-11eb-83f0-06358a409ac0", "I loved it!!!");
