CREATE OR ALTER PROCEDURE GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        FullName,
        Email,
        PasswordHash,
        CreatedAt,
        UpdatedAt
    FROM Users;
END;