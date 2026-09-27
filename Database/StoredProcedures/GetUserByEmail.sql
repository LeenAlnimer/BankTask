CREATE OR ALTER PROCEDURE GetUserByEmail
    @Email NVARCHAR(255)
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
    WHERE Email = @Email;
END;