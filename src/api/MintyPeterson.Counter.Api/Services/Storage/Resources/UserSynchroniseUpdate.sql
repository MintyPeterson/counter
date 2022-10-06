--
-- DESCRIPTION
--   Updates a user in the users table.
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
UPDATE
  Users WITH (UPDLOCK, SERIALIZABLE)
SET
  Users.UpdatedDateTime = @UpdatedDateTime
 ,Users.[Name] = @Name
 ,Users.Email = @Email
WHERE
  Users.UserID = @UserID