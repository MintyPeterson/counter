--
-- DESCRIPTION
--   Inserts a new entry into the entries table.
--
-- PARAMETERS
--   @CreatedDateTime - The time the entry was created.
--   @CreatedByUserID - The identifier of the user who created the entry.
--   @EntryDate - The entry date.
--   @Entry - The entry.
--   @Notes - Any notes or comments.
--
-- OUTPUTS
--   EntryID - The identifier of the newly created entry.
--
INSERT INTO Entries (
  EntryID
 ,CreatedDateTime
 ,CreatedByUserID
 ,UpdatedDateTime
 ,UpdatedByUserID
 ,EntryDate
 ,[Entry]
 ,Notes
)
OUTPUT
  Inserted.EntryID
VALUES (
  NEWID()
 ,@CreatedDateTime
 ,@CreatedByUserID
 ,@CreatedDateTime
 ,@CreatedByUserID
 ,@EntryDate
 ,@Entry
 ,@Notes
)