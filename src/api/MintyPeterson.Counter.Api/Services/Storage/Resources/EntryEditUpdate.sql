--
-- DESCRIPTION
--   Updates an entry in the entries table.
--
-- PARAMETERS
--   @EntryID - The entry identifier.
--   @UpdatedDateTime - The time the entry was created.
--   @UpdatedByUserID - The identifier of the user who created the entry.
--   @EntryDate - The entry date.
--   @Entry - The entry.
--   @Notes - Any notes or comments.
--   @IsEstimate - A value indicating whether the entry is an estimate or not.
--
-- OUTPUTS
--   EntryID - The identifier of the updated entry.
--
DECLARE @Output TABLE (
  EntryID uniqueidentifier
)

UPDATE
  Entries
SET
  Entries.UpdatedDateTime = @UpdatedDateTime
 ,Entries.UpdatedByUserID = @UpdatedByUserID
 ,Entries.EntryDate = @EntryDate
 ,Entries.[Entry] = @Entry
 ,Entries.Notes = @Notes
 ,Entries.IsEstimate = @IsEstimate
OUTPUT
  Inserted.EntryID
INTO
  @Output
WHERE
  Entries.EntryID = @EntryID
    AND
      Entries.DeletedDateTime IS NULL

SELECT
  EntryID
FROM
  @Output