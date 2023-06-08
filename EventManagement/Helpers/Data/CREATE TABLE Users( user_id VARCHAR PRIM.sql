---Creating User Table Schema
CREATE TABLE Users( 
    user_id VARCHAR(255)PRIMARY KEY,
    first_name varchar(100) NOT NULL
);
---Createing Event table Schema
CREATE TABLE Event (
    Event_Id VARCHAR(255) PRIMARY KEY,
    Event_Name VARCHAR(50) NOT NULL UNIQUE,
    Event_Start_DateTime DATETIME ,
    Event_End_DateTime DATETIME ,
);
---Createing Event Attendee table Schema
CREATE TABLE EventAttendee(
    Id VARCHAR(255) PRIMARY KEY,
    User_Id VARCHAR(255),
    Event_Id VARCHAR(255),
    FOREIGN KEY (User_Id) REFERENCES Users(user_id),FOREIGN KEY (Event_Id) REFERENCES Event(Event_Id)

);
INSERT INTO Users(user_id,first_name) VALUES('5f32b093-bbcd-4d07-b452-a4645834181c','Naveena')
INSERT INTO Users(user_id,first_name) VALUES('6cae82f9-2e43-4258-b65d-37e1a1bd629a','Abinaya')
DROP TABLE Users;
ALTER TABLE Users MODIFY COLUMN user_id VARCHAR MAX;
CREATE TABLE EventAttendee(
    Id VARCHAR(255) PRIMARY KEY,
    User_Id VARCHAR(255),
    Event_Id VARCHAR(255),
    FOREIGN KEY (User_Id) REFERENCES Users(user_id),FOREIGN KEY (Event_Id) REFERENCES Event(Event_Id)

);
INSERT INTO EventAttendee (Id,UserId,EventId) VALUES ('5f32b093-bbcd-4d07-b452-a4645834181c', '6cae82f9-2e43-4258-b65d-37e1a1bd629a', '46869fd3-c68d-486d-a73d-d448f5012889')
DROP TABLE Event
DROP TABLE EventAttendee
INSERT INTO Users (user_id, first_name) VALUES ('eacc4c3c-1865-4e6e-92f1-6f6d1c55c902','Naveena')
SELECT*from Users
SELECT*from Event
SELECT*FROM EventAttendee
TRUNCATE TABLE EventAttendee
TRUNCATE TABLE Event
TRUNCATE TABLE USERS
SELECT * FROM EventAttendee where User_Id='eacc4c3c-1865-4e6e-92f1-6f6d1c55c902'