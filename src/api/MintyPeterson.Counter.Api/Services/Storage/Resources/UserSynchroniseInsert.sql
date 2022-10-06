--
-- DESCRIPTION
--   Inserts a user in the users table.
--
-- PARAMETERS
--   @UserID - The user subject identifier.
--   @Name - The full name.
--   @Email - The e-mail address.
--   @UpdatedDateTime - The time the user was updated.
--
-- OUTPUTS
--   None
--
INSERT INTO Users (
  UserID
 ,[Name]
 ,Email
 ,CreatedDateTime
 ,UpdatedDateTime
)
VALUES(
  @UserID
 ,@Name
 ,@Email
 ,@UpdatedDateTime
 ,@UpdatedDateTime
)