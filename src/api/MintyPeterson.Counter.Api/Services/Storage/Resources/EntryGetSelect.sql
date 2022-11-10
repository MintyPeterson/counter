--
-- DESCRIPTION
--   Selects an entry.
--
-- PARAMETERS
--   @EntryID - The entry identifier.
--
-- OUTPUTS
--   EntryID - The entry identifier.
--   CreatedDateTime - The time the entry was created.
--   CreatedByUserID - The identifer of the user who created the entry.
--   UpdatedDateTime - The time the entry was updated.
--   UpdatedByUserID - The identifer of the user who updated the entry.
--   EntryDate -  The entry date.
--   Entry - The entry.
--   Notes - Any notes or comments.
--
SELECT
  Entries.EntryID
 ,Entries.CreatedDateTime
 ,Entries.CreatedByUserID
 ,Entries.UpdatedDateTime
 ,Entries.UpdatedByUserID
 ,Entries.EntryDate
 ,Entries.[Entry]
 ,Entries.Notes
FROM
  Entries
WHERE
  Entries.EntryID = @EntryID
    AND
      Entries.DeletedDateTime IS NULL