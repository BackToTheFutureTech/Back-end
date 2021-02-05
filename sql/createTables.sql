USE MADdays;

DROP TABLE IF EXISTS
	Picture, Comment, Opportunity_Volunteer, Opportunity, Volunteer, Charity, TaskImage 
;

CREATE TABLE Charity (
	charityId VARCHAR(40) NOT NULL,
	charityName VARCHAR(255),
	imageUrl VARCHAR(255),
	charityDescription VARCHAR(500),
	PRIMARY KEY (charityId)
);

CREATE TABLE TaskImage (
	taskType ENUM('Wrap Presents','Gardening','Sort Clothes','Serve Food','Other'),
	taskImageUrl VARCHAR(255),
	PRIMARY KEY (taskType)
);

CREATE TABLE Volunteer (
	volunteerId VARCHAR(40) NOT NULL,
	volunteerName VARCHAR(100),
	email VARCHAR(250),
	mobile CHAR(12),
	PRIMARY KEY (volunteerId)
);

CREATE TABLE Opportunity (
	opportunityId INT NOT NULL AUTO_INCREMENT,
	charityId VARCHAR(40),
	opportunityName VARCHAR(255),
	taskType ENUM('Wrap Presents','Gardening','Sort Clothes','Serve Food','Other'),
	numVolunteers INT,
	opportunityDate DATE,
	postcode VARCHAR(45),
	address1 VARCHAR(100),
	address2 VARCHAR(100),
	city VARCHAR(100),
	isActive BOOLEAN DEFAULT TRUE,
	thumbnail VARCHAR(255),
	opportunityDescription VARCHAR(500),
	PRIMARY KEY (opportunityId),
	FOREIGN KEY (charityId) REFERENCES Charity (charityId)
		ON DELETE CASCADE,
	FOREIGN KEY (taskType) REFERENCES TaskImage (taskType)
);


CREATE TABLE Opportunity_Volunteer (
	opportunityId INT NOT NULL,
	volunteerId VARCHAR(40) NOT NULL,
	groupNum INT DEFAULT 1,
	PRIMARY KEY (opportunityId, volunteerId),
	FOREIGN KEY (opportunityId) REFERENCES Opportunity (opportunityId)
		ON DELETE CASCADE,
	FOREIGN KEY (volunteerId) REFERENCES Volunteer (volunteerId)
		ON DELETE CASCADE
);

CREATE TABLE Comment (
	commentId INT NOT NULL AUTO_INCREMENT,
	charityId VARCHAR(40),
	comment VARCHAR(500),
	commentDate DATE,
	PRIMARY KEY (commentId),
	FOREIGN KEY (charityId) REFERENCES Charity (charityId)
		ON DELETE CASCADE
);

CREATE TABLE Picture (
	PictureId INT NOT NULL AUTO_INCREMENT,
	commentId INT,
	volunteerImageUrl VARCHAR(255),
	PRIMARY KEY (PictureId),
	FOREIGN KEY (commentId) REFERENCES Comment (commentId)
		ON DELETE CASCADE
);

CREATE OR REPLACE VIEW V_allOpportunities
AS SELECT   o.opportunityId id, 
            c.charityName charity, 
            o.opportunityName name, 
            o.taskType, 
            o.numVolunteers, 
            o.opportunityDate date, 
            o.postcode, 
            o.address1, 
            o.address2, 
            o.city, 
            o.opportunityDescription description,
			o.thumbnail,
            IFNULL(SUM(v.groupNum),0) AS numRegVolunteers 
FROM Opportunity o
JOIN Charity c 
ON  c.charityId = o.charityId
LEFT JOIN Opportunity_Volunteer v
ON o.opportunityId = v.opportunityId
WHERE o.isActive = TRUE
GROUP BY o.opportunityId
;

CREATE OR REPLACE VIEW V_Comment
AS SELECT   c.commentId id,
            c.charityId charityId,
            c.comment comment,
            p.volunteerImageUrl imageUrl
FROM Comment c 
LEFT JOIN Picture p 
ON c.commentId = p.commentId
;

