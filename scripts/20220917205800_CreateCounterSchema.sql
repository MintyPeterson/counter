-- <copyright file="CreateSchema.sql" company="Tom Cook">
-- Copyright (c) Tom Cook. All rights reserved.
-- </copyright>

--
-- DESCRIPTION
--   This script creates the database schema and populates the reference tables.
--   WARNING! All data is deleted before creating the schema.
--

DROP TABLE IF EXISTS Entries
DROP TABLE IF EXISTS Users

CREATE TABLE Users (
  UserID nvarchar(450) NOT NULL PRIMARY KEY
 ,CreatedDateTime datetimeoffset NOT NULL
 ,UpdatedDateTime datetimeoffset NOT NULL
 ,DeletedDateTime datetimeoffset
 ,[Name] nvarchar(MAX)
 ,Email nvarchar(320) NOT NULL
)

CREATE TABLE Entries (
  EntryID uniqueidentifier NOT NULL PRIMARY KEY
 ,CreatedDateTime datetimeoffset NOT NULL
 ,CreatedByUserID nvarchar(450) NOT NULL REFERENCES Users (UserID)
 ,UpdatedDateTime datetimeoffset NOT NULL
 ,UpdatedByUserID nvarchar(450) NOT NULL REFERENCES Users (UserID)
 ,DeletedDateTime datetimeoffset
 ,DeletedByUserID nvarchar(450) REFERENCES Users (UserID)
 ,EntryDate date NOT NULL
 ,[Entry] decimal(5, 0) NOT NULL
 ,Notes nvarchar(MAX)
)