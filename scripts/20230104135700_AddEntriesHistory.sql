-- <copyright file="AddEntriesHistory.sql" company="Tom Cook">
-- Copyright (c) Tom Cook. All rights reserved.
-- </copyright>

--
-- DESCRIPTION
--   This script creates an audit history table and trigger for
--   entry data.
--

CREATE TABLE EntriesHistory (
  AuditDateTime datetimeoffset NOT NULL
 ,AuditAction char(1) NOT NULL
 ,AuditApplication nvarchar(128)
 ,EntryID uniqueidentifier
 ,CreatedDateTime datetimeoffset
 ,CreatedByUserID nvarchar(450)
 ,UpdatedDateTime datetimeoffset
 ,UpdatedByUserID nvarchar(450)
 ,DeletedDateTime datetimeoffset
 ,DeletedByUserID nvarchar(450)
 ,EntryDate date
 ,[Entry] decimal(5, 0)
 ,Notes nvarchar(MAX)
 ,IsEstimate bit
)

GO

CREATE TRIGGER SaveEntriesHistory ON Entries AFTER INSERT, UPDATE, DELETE
AS
  SET NOCOUNT ON
  SET XACT_ABORT ON
  SET ARITHABORT ON

  DECLARE @AuditAction char(1) = (
    CASE
      WHEN EXISTS (SELECT * FROM Inserted)
        AND EXISTS (SELECT * FROM Deleted)
          THEN 'U'
      WHEN EXISTS (SELECT * FROM Inserted)
        THEN 'I'
      WHEN EXISTS (SELECT * FROM Deleted)
        THEN 'D'
      ELSE NULL
    END
  )

  IF @AuditAction IN ('I', 'U')
  BEGIN
    INSERT INTO
      EntriesHistory
    SELECT
      AuditDateTime = SYSDATETIMEOFFSET()
     ,AuditAction = @AuditAction
     ,AuditApplication = APP_NAME()
     ,Inserted.*
    FROM
      Inserted
  END
  ELSE
  BEGIN
    INSERT INTO
      EntriesHistory
    SELECT
      AuditDateTime = SYSDATETIMEOFFSET()
     ,AuditAction = @AuditAction
     ,AuditApplication = APP_NAME()
     ,Deleted.*
    FROM
      Deleted
  END