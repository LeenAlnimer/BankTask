CREATE OR ALTER PROCEDURE GetUserById
    @Id UNIQUEIDENTIFIER
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
    FROM Users
    WHERE Id = @Id;
END;