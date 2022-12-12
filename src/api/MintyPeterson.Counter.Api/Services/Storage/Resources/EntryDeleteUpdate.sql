--
-- DESCRIPTION
--   Updates an entry in the entries table to mark the
--   row as deleted.
--
-- PARAMETERS
--   @EntryID - The entry identifier.
--   @DeletedDateTime - The time the entry was deleted.
--   @DeletedByUserID - The identifier of the user who deleted the entry.
--
-- OUTPUTS
--   EntryID - The identifier of the updated entry.
--
UPDATE
  Entries
SET
  Entries.DeletedDateTime = @DeletedDateTime
 ,Entries.DeletedByUserID = @DeletedByUserID
OUTPUT
  Inserted.EntryID
WHERE
  Entries.EntryID = @EntryID
    AND
      Entries.DeletedDateTime IS NULL